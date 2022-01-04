using Character.Enemy;
using UnityEngine;


namespace HeroicOpportunity.Services.Enemies
{
    public interface IEnemiesService
    {
        BaseEnemyController CreateEnemy(string enemyId, Transform parent);
    }
}
