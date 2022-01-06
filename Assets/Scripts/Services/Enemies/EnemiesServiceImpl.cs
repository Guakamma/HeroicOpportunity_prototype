using System;
using System.Collections.Generic;
using System.Linq;
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


        public BaseEnemyController CreateEnemy(EnemyType type, Transform parent)
        {
            EnemyInfo enemyInfo = _enemyInfos.Values.FirstOrDefault(i => i.EnemyType == type);
            return CreateEnemy(enemyInfo, parent);
        }
        
        public BaseEnemyController CreateEnemy(EnemyType type, Transform parent, Vector3 position)
        {
            EnemyInfo enemyInfo = _enemyInfos.Values.FirstOrDefault(i => i.EnemyType == type);

            if (enemyInfo == null)
            {
                Debug.LogException(new Exception($"{type} is unknown enemy type, trying to create unregistered enemy"));
                return null;
            }
            
            return CreateEnemy(enemyInfo, parent, position);
        }

        private BaseEnemyController CreateEnemy(EnemyInfo enemyInfo, Transform parent, Vector3? spawnPosition = null)
        {
            Vector3 position;
            if (spawnPosition != null)
            {
                position = spawnPosition.Value;
            }
            else
            {
                position = _heroService.ActiveHero.transform.position;
                position.z += enemyInfo.HeroDistance;
            }
            
            Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));

            // BaseEnemyController enemy = new GameObject(nameof(BaseEnemyController) + "_" + enemyInfo.Id)
            //     .AddComponent<BaseEnemyController>();

            BaseEnemyController enemy = CreateEnemy(enemyInfo);

            enemy.transform.SetParent(parent);
            enemy.transform.SetPositionAndRotation(position, rotation);
            enemy.Initialize(enemyInfo);

            return enemy;
        }

        private static BaseEnemyController CreateEnemy(EnemyInfo enemyInfo)
        {
            BaseEnemyController enemy;
            if (enemyInfo.IsBoss)
                return new GameObject(nameof(StaticBossEnemyController) + "_" + enemyInfo.Id).AddComponent<StaticBossEnemyController>();
            else
                return new GameObject(nameof(BaseEnemyController) + "_" + enemyInfo.Id).AddComponent<BaseEnemyController>();
        }
    }
}
