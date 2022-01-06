using HeroicOpportunity.Character;
using Services;
using UniRx;
using UnityEngine;

namespace Character.Damage
{
    public class AbilityDamage : MonoBehaviour
    {
        #region Fields

        private CompositeDisposable _disposables = new CompositeDisposable();

        private ICharacter _character;

        #endregion



        #region Public methods

        public void Initialize(ICharacter character)
        {
            // _disposables = new CompositeDisposable();
            _character = character;

            // ServicesHub.Events.Ability.AbilityDamage
            //     .Subscribe(a => _character.GetDamaged(a.Damage))
            //     .AddTo(this)
            //     .AddTo(_disposables);

            ServicesHub.Events.Ability.AbilityComboDamage
                .Subscribe(damage => _character.GetDamaged(damage))
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
