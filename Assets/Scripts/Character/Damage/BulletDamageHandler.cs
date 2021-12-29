using HeroicOpportunity.Gun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;


namespace HeroicOpportunity.Character
{
    public class BulletDamageHandler : MonoBehaviour
    {
        #region Fields

        private CompositeDisposable _disposables = new CompositeDisposable();
        private ICharacter _character;

        #endregion



        #region Public methods

        public void Initialize(Collider collider, ICharacter character)
        {
            _character = character;
            collider.OnTriggerEnterAsObservable()
                .Select(c => c.GetComponent<Bullet>())
                .Where(b => b != null)
                .Where(b => b.Sender != _character)
                .Subscribe(bullet =>
                {
                    int damage = bullet.Damage;
                    _character.GetDamaged(damage);
                    bullet.Dispose();
                })
                .AddTo(this)
                .AddTo(_disposables);
        }


        public void Dispose()
        {
            _disposables.Clear();
        }

        #endregion
    }
}
