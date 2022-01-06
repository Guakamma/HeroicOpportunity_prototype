using HeroicOpportunity.Character;
using Services;
using UniRx;
using UnityEngine;

namespace Character.Damage
{
    public class AbilityDamage : MonoBehaviour
    {
        #region Fields

        private CompositeDisposable _disposables;

        private ICharacter _character;

        #endregion



        #region Public methods

        public void Initialize(Collider heroCollider, ICharacter character)
        {
            _disposables = new CompositeDisposable();
            _character = character;

            // ServicesHub.Events.Ability.AbilityComboDamage
            //     .Subscribe(damage => _character.GetDamage(damage))
            //     .AddTo(this)
            //     .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Clear();
        }

        #endregion
    }
}
