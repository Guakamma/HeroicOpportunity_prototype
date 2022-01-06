using System;
using System.Collections.Generic;
using Character.Damage;
using Data.Enemies;
using HeroicOpportunity.Character;
using HeroicOpportunity.Game;
using HeroicOpportunity.Gun;
using Services;
using UniRx;
using UnityEngine;

namespace Character.Enemy
{
    public class BaseEnemyController : MonoBehaviour, ICharacter
    {
        protected List<GunsController> _gunsControllers;
        private CompositeDisposable _disposables;
        private CharacterModel _characterModel;
        private BulletDamageHandler _bulletDamageHandler;
        //private AbilityDamage _abilityDamage;
        
        private int _health;


        private bool CanMove => gameObject.activeSelf && GameManager.Instance.CurrentState == GameStateType.InGame;
        public EnemyInfo EnemyInfo { get; private set; }

        public bool IsDied => Health <= 0;

        
        public virtual void Initialize(EnemyInfo enemyInfo)
        {
            EnemyInfo = enemyInfo;
            _disposables = new CompositeDisposable();

            _characterModel = Instantiate(EnemyInfo.CharacterModel, Root.transform);

            Health = EnemyInfo.Health;
            _characterModel.HealthBar.Initialize(this, EnemyInfo.Health);
            _characterModel.Initialize(this);

            _gunsControllers = new List<GunsController>();
            foreach (var root in _characterModel.GunRoots)
            {
                GunsController gunsController = root.gameObject.AddComponent<GunsController>();
                gunsController.Initialize(EnemyInfo.GunInfo, this);
                _gunsControllers.Add(gunsController);
            }
            
            SetPositionFromHero();

            Observable.EveryUpdate()
                .Subscribe(_ => Move(enemyInfo))
                .AddTo(this);
            
            GameStateController.OnStateChanged
                .Subscribe(OnStateChanged)
                .AddTo(_disposables)
                .AddTo(this);

            _bulletDamageHandler = gameObject.AddComponent<BulletDamageHandler>();
            //_abilityDamage = gameObject.AddComponent<AbilityDamage>();
        }

        protected virtual void Move(EnemyInfo enemyInfo)
        {
            if (!CanMove)
                return;

            float speed = enemyInfo.IsBoss ? ServicesHub.Hero.ActiveHero.Speed : enemyInfo.Speed;
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }


        public virtual void Dispose()
        {
            Hide();

            foreach (var gc in _gunsControllers)
            {
                gc.Dispose();
            }

            Destroy(gameObject);
        }


        public void Show()
        {
            Observable.Timer(TimeSpan.FromSeconds(0.5))
                .Subscribe(_ =>
                {
                    ServicesHub.Events.Enemy.ShowEnemy(this);
                    gameObject.SetActive(true);
                    _bulletDamageHandler.Initialize(_characterModel.Collider, this);
                    //_abilityDamage.Initialize(_characterModel.Collider, this);
                    SetPositionFromHero();
                })
                .AddTo(this);
        }

        private void SetPositionFromHero()
        {
            Vector3 position = transform.position;
            position = new Vector3(position.x, position.y, ServicesHub.Hero.ActiveHero.PositionZ + EnemyInfo.HeroDistance); 
            transform.position = position;
        }


        public virtual void Hide()
        {
            _bulletDamageHandler.Dispose();
            //_abilityDamage.Dispose();
            SetIsShoot(false);
            gameObject.SetActive(false);
        }


        private void Dead()
        {
            ServicesHub.Events.Enemy.DieEnemy(this);
            Dispose();
        }


        private void ChangePosition()
        {
            
        }


        protected virtual void OnStateChanged(GameStateType gameStateType)
        {
            switch (gameStateType)
            {
                case GameStateType.InGame:
                    SetIsShoot(true);
                    break;
                default:
                    SetIsShoot(false);
                    break;
            }
        }


        public void SetIsShoot(bool value)
        {
            foreach (var g in _gunsControllers)
            {
                if (value)
                {
                    g.StartShoot();
                }
                else
                {
                    g.StopShoot();
                }
            }
        }


        public GameObject Root => gameObject;
        public int Damage => EnemyInfo.Damage;


        public int Health
        {
            get => _health;
            private set => _health = Mathf.Clamp(value, 0, EnemyInfo.Health);
        }


        public void GetDamage(int value)
        {
            Health -= value;

            if (IsDied)
            {
                Dead();
            }
        }
    }
}
