using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Enemies;
using HeroicOpportunity.Character.Enemy;
using System.Collections.Generic;
using HeroicOpportunity.Services.Hero;
using UnityEngine;


namespace HeroicOpportunity.Services.Enemies
{
    public class EnemiesServiceImpl : IEnemiesService
    {
        #region Fields

        private readonly IHeroService _heroService;
        private readonly Dictionary<string, EnemyInfo> _enemyInfos;

        #endregion



        #region Properties

        private EnemiesData EnemiesData => DataHub.Enemies;

        #endregion



        #region Class lifecycle

        public EnemiesServiceImpl(IHeroService heroService)
        {
            _heroService = heroService;

            _enemyInfos = new Dictionary<string, EnemyInfo>();
            foreach (var p in EnemiesData.EnemiesInfoPaths)
            {
                _enemyInfos.Add(p.Key, Resources.Load<EnemyInfo>(p.Value));
            }
        }

        #endregion



        #region IEnemiesService

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

        #endregion
    }
}
