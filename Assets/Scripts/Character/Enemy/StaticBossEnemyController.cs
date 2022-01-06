using System;
using Data.Enemies;
using HeroicOpportunity.Gun;
using Input;
using Services;
using UniRx;
using UnityEngine;

namespace Character.Enemy
{
    public class StaticBossEnemyController : BaseEnemyController
    {
        private float _followGunsDelay = 1f;

        public override void Initialize(EnemyInfo enemyInfo)
        {
            base.Initialize(enemyInfo);

             ServicesHub.Events.Hero.DirectionChanged
                .Subscribe(ChangeGunsDirection)
                .AddTo(this);
             
        }

        private void ChangeGunsDirection((Direction direction, float value) delta)
        {
            Observable.Timer(TimeSpan.FromSeconds(_followGunsDelay))
                .Subscribe(_ => ChangeGunsDirection(delta.direction, delta.value))
                .AddTo(this);
        }

        private void ChangeGunsDirection(Direction direction, float delta)
        {
            foreach (GunsController gun in _gunsControllers)
            {
                gun.transform.position += new Vector3(delta, 0, 0);
            }
        }
    }
}