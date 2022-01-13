using HeroicOpportunity.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character
{
    public class CharacterModel : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _gunRoots;

        [SerializeField] [Required]
        private Collider _collider;

        [SerializeField] [Required]
        private HealthBar _healthBar;
        
        [SerializeField] [Required]
        private CharacterFxHandler _fxHandler;

        private ICharacter _character;


        public Transform[] GunRoots => _gunRoots;
        public Collider Collider => _collider;
        public HealthBar HealthBar => _healthBar;
        public CharacterFxHandler FxHandler => _fxHandler;

        public void Initialize(ICharacter character)
        {
            _character = character;
            _fxHandler.Initialize();
        }

        public void GetDamage(int damage)
        {
            if (_character != null)
                _character.GetDamage(damage);
        }
    }
}
