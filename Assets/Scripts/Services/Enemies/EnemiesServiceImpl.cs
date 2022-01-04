using System.Collections.Generic;
using Character.Enemy;
using Data.Enemies;
using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Enemies;
using HeroicOpportunity.Services.Enemies;
using HeroicOpportunity.Services.Hero;
using UnityEngine;

namespace Services.Enemies
{
    public class EnemiesServiceImpl : IEnemiesService
    {
        private readonly IHeroService _heroService;
        private readonly Dictionary<string, EnemyInfo> _enemyInfos;


        private EnemiesData EnemiesData => DataHub.Enemies;


        public EnemiesServiceImpl(IHeroService heroService)
        {
            _heroService = heroService;

            _enemyInfos = new Dictionary<string, EnemyInfo>();
            foreach (var p in EnemiesData.EnemiesInfoPaths)
            {
                _enemyInfos.Add(p.Key, Resources.Load<EnemyInfo>(p.Value));
            }
        }


        public BaseEnemyController CreateEnemy(string enemyId, Transform parent)
        {
            EnemyInfo enemyInfo = _enemyInfos[enemyId];
            BaseEnemyController enemy = new GameObject(nameof(BaseEnemyController) + "_" + enemyInfo.Id)
                .AddComponent<BaseEnemyController>();
            enemy.transform.SetParent(parent);

            Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
            Vector3 position = _heroService.ActiveHero.transform.position;
            position.z -= enemyInfo.HeroDistance * 2.0f;
            enemy.transform.SetPositionAndRotation(position, rotation);

            enemy.Initialize(enemyInfo);

            return enemy;
        }
    }
}
