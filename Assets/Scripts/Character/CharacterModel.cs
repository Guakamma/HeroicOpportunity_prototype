using Character;
using Sirenix.OdinInspector;
using UnityEngine;


namespace HeroicOpportunity.Character
{
    public class CharacterModel : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _gunRoots;

        [SerializeField] [Required]
        private Collider _collider;

        [SerializeField] [Required]
        private HealthBar _healthBar;

        private ICharacter _character;


        public Transform[] GunRoots => _gunRoots;
        public Collider Collider => _collider;
        public HealthBar HealthBar => _healthBar;

        public void Initialize(ICharacter character)
        {
            _character = character;
        }

        public void GetDamage(int damage)
        {
            if (_character != null)
                _character.GetDamage(damage);
        }
    }
}
