using Character.Enemy;
using HeroicOpportunity.Data.Enemies;
using UnityEngine;


namespace HeroicOpportunity.Services.Enemies
{
    public interface IEnemiesService
    {
        BaseEnemyController CreateEnemy(EnemyType type, Transform parent);
        BaseEnemyController CreateEnemy(EnemyType type, Transform parent, Vector3 position);
    }
}
