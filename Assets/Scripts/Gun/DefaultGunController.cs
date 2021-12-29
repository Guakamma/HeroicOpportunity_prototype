using System;
using DG.Tweening;
using HeroicOpportunity.Character;
using HeroicOpportunity.Data;
using UniRx;
using UnityEngine;


namespace HeroicOpportunity.Gun
{
    public class GunsController : MonoBehaviour
    {
        #region Fields

        private GunInfo _gunInfo;
        private ICharacter _sender;
        private ObjectsPool<Bullet> _bulletsPool;
        private Sequence _shootSequence;


        #endregion



        #region Public methods

        public void Initialize(GunInfo gunInfo, ICharacter sender)
        {
            _gunInfo = gunInfo;
            _sender = sender;
            _bulletsPool = new ObjectsPool<Bullet>(_gunInfo.BulletPrefab);
            _shootSequence = DOTween.Sequence()
                .SetTarget(gameObject)
                .AppendInterval(_gunInfo.FireRate)
                .AppendCallback(() =>
                {
                    Bullet bullet = _bulletsPool.GetObject(transform.position, transform.rotation);
                    bullet.Initialize(_sender, gunInfo);
                })
                .SetLoops(-1);
            StopShoot();
        }


        public void StartShoot()
        {
            _shootSequence.Restart();
            _shootSequence.Play();
        }


        public void StopShoot()
        {
            _shootSequence.Pause();
        }


        public void Dispose()
        {
            StopShoot();

            Observable.Timer(TimeSpan.FromSeconds(_gunInfo.FireRate))
                .Subscribe(_ => _bulletsPool.DestroyPool());
        }

        #endregion
    }
}
