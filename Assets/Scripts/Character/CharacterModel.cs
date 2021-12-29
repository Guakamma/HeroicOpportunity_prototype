using Sirenix.OdinInspector;
using UnityEngine;


namespace HeroicOpportunity.Character
{
    public class CharacterModel : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform[] _gunRoots;

        [SerializeField] [Required]
        private Collider _collider;

        [SerializeField] [Required]
        private HealthBar _healthBar;

        #endregion



        #region Properties

        public Transform[] GunRoots => _gunRoots;
        public Collider Collider => _collider;
        public HealthBar HealthBar => _healthBar;

        #endregion
    }
}
