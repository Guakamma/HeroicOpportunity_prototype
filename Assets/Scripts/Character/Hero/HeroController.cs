using System;
using System.Collections.Generic;
using System.Linq;
using Character.Damage;
using Character.Enemy;
using DG.Tweening;
using HeroicOpportunity.Character;
using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Data.Heroes;
using HeroicOpportunity.Game;
using HeroicOpportunity.Gun;
using HeroicOpportunity.Services.Events;
using HeroicOpportunity.Services.Level;
using Input;
using Services;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Character.Hero
{
    public class HeroController : MonoBehaviour, ICharacter
    {
        #region Fields

        private HeroInfo _heroInfo;
        private CompositeDisposable _disposables;
        private int _health;
        private float _speed;
        private List<GunsController> _gunsControllers;

        #endregion



        #region Properties

        public bool IsRun => Speed > 0.0f;


        public float Speed
        {
            get => _speed;
            private set => _speed = Mathf.Clamp(value, 0.0f, float.MaxValue);
        }

        public float PositionZ => transform.position.z;


        private ILevelService LevelService => ServicesHub.Level;

        #endregion



        #region Public methods

        public void Initialize(HeroInfo heroInfo)
        {
            _heroInfo = heroInfo;
            _disposables = new CompositeDisposable();

            CharacterModel characterModel = Instantiate(_heroInfo.CharacterModel, Vector3.zero, Quaternion.identity, Root.transform);
            Health = _heroInfo.Health;
            characterModel.HealthBar.Initialize(this, _heroInfo.Health);

            _gunsControllers = new List<GunsController>();
            foreach (var r in characterModel.GunRoots)
            {
                GunsController gunsController = r.gameObject.AddComponent<GunsController>();
                gunsController.Initialize(_heroInfo.GunInfo, this);
                _gunsControllers.Add(gunsController);
            }

            Vector3 position = transform.position;
            position.x = LevelService.ActiveLevel.GetMiddlePositionX();
            transform.position = position;

            Observable.EveryUpdate()
                .Subscribe(_ => Move())
                .AddTo(_disposables)
                .AddTo(this);

            IEventsService eventsService = ServicesHub.Events;
            eventsService.Input.DirectionChanged
                .Where(_ => IsRun)
                .Subscribe(ChangeRoad)
                .AddTo(_disposables)
                .AddTo(this);

            GameStateController.OnStateChanged
                .Subscribe(OnStateChanged)
                .AddTo(_disposables)
                .AddTo(this);

            ServicesHub.Events.Ability.AbilityComboDamage
                .Subscribe((damage) =>
                {
                    if (damage > 0)
                        HealUp();
                })
                .AddTo(this)
                .AddTo(_disposables);
            
            ServicesHub.Events.Ability.AbilityUse
                .Subscribe(UseDamageAbility)
                .AddTo(this)
                .AddTo(_disposables);

            BulletDamageHandler bulletDamageHandler = gameObject.AddComponent<BulletDamageHandler>();
            bulletDamageHandler.Initialize(characterModel.Collider, this);
        }

        private void UseDamageAbility(AbilityInfo abilityInfo)
        {
            RaycastHit[] hits = new RaycastHit[10];
            Physics.RaycastNonAlloc(transform.position, transform.forward, hits, 50f);

            List<CharacterModel> enemies = new List<CharacterModel>(1);
            Transform enemyTransform;
            foreach (RaycastHit hit in hits)
            {
                enemyTransform = hit.transform;
                if (enemyTransform != null && enemyTransform.TryGetComponent(out CharacterModel enemy)) 
                    enemies.Add(enemy);
            }

            float lastDistance = float.MaxValue;
            CharacterModel enemyModel = enemies.First();
            foreach (CharacterModel enemy in enemies)
            {
                float distance = enemy.transform.position.z - transform.position.z;
                if (lastDistance > distance)
                {
                    lastDistance = distance; 
                    enemyModel = enemy;
                }
            }
            
            enemyModel.GetDamage(abilityInfo.Damage);
        }

        private void Move()
        {
            Vector3 director = Vector3.forward;
            transform.Translate(director * Speed * Time.deltaTime);
        }

        private void HealUp()
        {
            Health = _heroInfo.Health;
        }


        public void Dispose()
        {
            _disposables.Clear();

            foreach (var gc in _gunsControllers)
            {
                gc.Dispose();
            }

            Destroy(gameObject);
        }

        #endregion



        #region Private methods

        private void ChangeRoad(Direction direction)
        {
            float targetPositionX = transform.position.x;
            float step = LevelService.ActiveLevel.GetStepX();
            switch (direction)
            {
                case Direction.Right:
                    targetPositionX += step;
                    break;
                case Direction.Left:
                    targetPositionX -= step;
                    break;
                default:
                    return;
            }

            Vector2 minMaxPosition = LevelService.ActiveLevel.MinMaxPositionX();
            targetPositionX = Mathf.Clamp(targetPositionX, minMaxPosition.x, minMaxPosition.y);
            
            ServicesHub.Events.Hero.ChangeDirection(direction, targetPositionX - transform.position.x);
            
            transform.DOMoveX(targetPositionX, 0.3f)
                .SetEase(Ease.InQuint);
        }


        private void OnStateChanged(GameStateType gameStateType)
        {
            Debug.Log(gameStateType.ToString());
            switch (gameStateType)
            {
                case GameStateType.InGame:
                    Speed = _heroInfo.Speed;
                    SetIsShoot(true);
                    break;
                default:
                    Speed = 0.0f;
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

        #endregion



        #region ICharacter

        public GameObject Root => gameObject;
        public int Damage => _heroInfo.Damage;


        public int Health
        {
            get => _health;
            private set => _health = Mathf.Clamp(value, 0, _heroInfo.Health);
        }


        public void GetDamage(int value)
        {
            Health -= value;

            if (_health <= 0)
            {
                GameManager.Instance.SetGameState(GameStateType.Result);
            }
        }

        #endregion
    }
}
