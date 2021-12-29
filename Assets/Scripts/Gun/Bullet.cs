using System;
using HeroicOpportunity.Character;
using HeroicOpportunity.Data;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;


namespace HeroicOpportunity.Gun
{
    public class Bullet : PoolObject
    {
        #region Fields

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        [SerializeField] [Required]
        private Collider _collider;

        private GunInfo _gunInfo;

        #endregion



        #region Properties

        public ICharacter Sender { get; private set; }
        public int Damage { get; private set; }

        #endregion



        #region Public methods

        public void Initialize(ICharacter sender, GunInfo gunInfo)
        {
            Sender = sender;
            _gunInfo = gunInfo;
            Damage = Sender.Damage;

            Observable.EveryUpdate()
                .Subscribe(_ => Move(_gunInfo.BulletSpeed))
                .AddTo(this)
                .AddTo(_disposables);

            Observable.Timer(TimeSpan.FromSeconds(_gunInfo.BulletLifetime))
                .Subscribe(_ => Dispose())
                .AddTo(this)
                .AddTo(_disposables);

            /*
            _collider.OnTriggerEnterAsObservable()
                .Subscribe(c =>
                {
                    Dispose();
                })
                .AddTo(this)
                .AddTo(_disposables);
            // */
        }

        #endregion



        #region Public methods

        public void Dispose()
        {
            _disposables.Clear();
            base.ReturnToPool();
        }

        #endregion



        #region Private methods

        private void Move(float speed)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        #endregion
    }
}
