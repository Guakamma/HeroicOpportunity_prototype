using HeroicOpportunity.Character;
using HeroicOpportunity.Gun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Character.Damage
{
    public class BulletDamageHandler : MonoBehaviour
    {
        private CompositeDisposable _disposables = new CompositeDisposable();
        private ICharacter _character;


        public void Initialize(Collider heroCollider, ICharacter character)
        {
            _character = character;
            heroCollider.OnTriggerEnterAsObservable()
                .Select(c => c.GetComponent<Bullet>())
                .Where(b => b != null)
                .Where(b => b.Sender != _character)
                .Subscribe(bullet =>
                {
                    int damage = bullet.Damage;
                    _character.GetDamage(damage);
                    bullet.Dispose();
                })
                .AddTo(this)
                .AddTo(_disposables);
        }


        public void Dispose()
        {
            _disposables.Clear();
        }
    }
}
