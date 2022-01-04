using System;
using System.Collections.Generic;
using Data.Enemies;
using HeroicOpportunity.Character;
using HeroicOpportunity.Data.Enemies;
using HeroicOpportunity.Game;
using HeroicOpportunity.Gun;
using HeroicOpportunity.Services.Hero;
using HeroicOpportunity.Services.Level;
using Services;
using UniRx;
using UnityEngine;

namespace Character.Enemy
{
    public class BaseEnemyController : MonoBehaviour, ICharacter
    {
        private CompositeDisposable _disposables;
        private int _health;
        private CharacterModel _characterModel;
        private List<GunsController> _gunsControllers;
        private BulletDamageHandler _bulletDamageHandler;
        private AbilityDamage _abilityDamage;


        public EnemyInfo EnemyInfo { get; private set; }

        public bool IsDied => Health <= 0;

        
        public void Initialize(EnemyInfo enemyInfo)
        {
            EnemyInfo = enemyInfo;
            _disposables = new CompositeDisposable();

            _characterModel = Instantiate(EnemyInfo.CharacterModel, Root.transform);

            Health = EnemyInfo.Health;
            _characterModel.HealthBar.Initialize(this, EnemyInfo.Health);

            _gunsControllers = new List<GunsController>();
            foreach (var r in _characterModel.GunRoots)
            {
                GunsController gunsController = r.gameObject.AddComponent<GunsController>();
                gunsController.Initialize(EnemyInfo.GunInfo, this);
                _gunsControllers.Add(gunsController);
            }


            ILevelService levelService = ServicesHub.Level;
            IHeroService heroService = ServicesHub.Hero;

            Vector3 position = transform.position;
            position.x = levelService.ActiveLevel.GetMiddlePositionX();
            transform.position = position;

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    Vector3 targetPosition = transform.position;
                    targetPosition.z = heroService.ActiveHero.transform.position.z + EnemyInfo.HeroDistance;
                    transform.position = targetPosition;
                })
                .AddTo(this);
            
            GameStateController.OnStateChanged
                .Subscribe(OnStateChanged)
                .AddTo(_disposables)
                .AddTo(this);

            _bulletDamageHandler = gameObject.AddComponent<BulletDamageHandler>();
            _abilityDamage = gameObject.AddComponent<AbilityDamage>();
        }


        public void Dispose()
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
                    _abilityDamage.Initialize(this);
                    SetIsShoot(true);
                })
                .AddTo(this);
        }


        public void Hide()
        {
            _bulletDamageHandler.Dispose();
            _abilityDamage.Dispose();
            SetIsShoot(false);
            gameObject.SetActive(false);
        }


        private void Dead()
        {
            ServicesHub.Events.Enemy.DieEnemy(this);
            Dispose();
        }


        private void ChangePosition() { }


        private void OnStateChanged(GameStateType gameStateType)
        {
            Debug.Log(gameStateType.ToString());
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


        private void SetIsShoot(bool value)
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


        public void GetDamaged(int value)
        {
            Health -= value;

            if (IsDied)
            {
                Dead();
            }
        }
    }
}
