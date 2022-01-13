using System.Collections.Generic;
using System.Linq;
using Character.Damage;
using Data.Heroes;
using DG.Tweening;
using Game;
using HeroicOpportunity.Character;
using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Game;
using HeroicOpportunity.Gun;
using HeroicOpportunity.Services.Events;
using HeroicOpportunity.Services.Level;
using Input;
using Services;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;

namespace Character.Hero
{
    public class HeroController : MonoBehaviour, ICharacter
    {
        private HeroInfo _heroInfo;
        private CompositeDisposable _disposables;
        private int _health;
        private float _speed;
        private List<GunsController> _gunsControllers;
        private CharacterModel _characterModel;
        private BulletDamageHandler _bulletDamageHandler;


        public bool IsRun => Speed > 0.0f;

        private bool CanTakeDamage { get; set; }

        public float Speed
        {
            get => _speed;
            private set => _speed = Mathf.Clamp(value, 0.0f, float.MaxValue);
        }

        public float PositionZ => transform.position.z;

        private ILevelService LevelService => ServicesHub.Level;


        public void Initialize(HeroInfo heroInfo)
        {
            CanTakeDamage = true;
            _heroInfo = heroInfo;
            _disposables = new CompositeDisposable();

            _characterModel = Instantiate(_heroInfo.CharacterModel, Vector3.zero, Quaternion.identity, Root.transform);
            _characterModel.Initialize(this);
            
            Health = _heroInfo.Health;
            _characterModel.HealthBar.Initialize(this, _heroInfo.Health);

            _gunsControllers = new List<GunsController>();
            foreach (var r in _characterModel.GunRoots)
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

                    ProtectHeroWithShield();
                })
                .AddTo(this)
                .AddTo(_disposables);
            
            ServicesHub.Events.Ability.AbilityUse
                .Subscribe(UseDamageAbility)
                .AddTo(this)
                .AddTo(_disposables);

            _bulletDamageHandler = gameObject.AddComponent<BulletDamageHandler>();
            _bulletDamageHandler.Initialize(_characterModel.Collider, this);
        }

        private void ProtectHeroWithShield()
        {
            CanTakeDamage = false;
            _characterModel.FxHandler.ShowFx(CharacterFxHandler.FxType.ProtectedShield, _heroInfo.ComboProtectDuration,
                () => CanTakeDamage = true);
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
            
            if (enemies.IsNullOrEmpty())
                return;

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
            
            CanTakeDamage = gameStateType == GameStateType.InGame;
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
            if (!CanTakeDamage)
                return;
            
            Health -= value;

            if (_health <= 0)
            {
                GameManager.Instance.SetGameState(GameStateType.Result);
            }
        }

        #endregion
    }
}
