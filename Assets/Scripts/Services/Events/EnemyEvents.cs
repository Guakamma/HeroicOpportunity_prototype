using System;
using Character.Enemy;
using UniRx;


namespace HeroicOpportunity.Services.Events
{
    public class EnemyEvents
    {
        private readonly Subject<BaseEnemyController> _enemyDied = new Subject<BaseEnemyController>();
        private readonly Subject<BaseEnemyController> _enemyShowed = new Subject<BaseEnemyController>();


        public IObservable<BaseEnemyController> EnemyDied => _enemyDied.AsObservable();
        public IObservable<BaseEnemyController> EnemyShowed => _enemyShowed.AsObservable();



        public void DieEnemy(BaseEnemyController enemy) => _enemyDied.OnNext(enemy);
        public void ShowEnemy(BaseEnemyController enemy) => _enemyShowed.OnNext(enemy);
    }
}
