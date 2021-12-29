using HeroicOpportunity.Character;
using Sirenix.OdinInspector;
using UnityEngine;


namespace HeroicOpportunity.Data.Heroes
{
    [CreateAssetMenu(fileName = PrefixName + "name", menuName = "Data/Heroes/Hero Info")]
    public class HeroInfo : ScriptableObject
    {
        #region Fields

        private const string PrefixName = "Data_Heroes_Infos_";

        [SerializeField] [Required]
        private CharacterModel _characterModel;
        [SerializeField] [Min(0)]
        private int _health;
        [SerializeField] [Min(0.0f)]
        private float _speed;
        [SerializeField] [Min(0)]
        private int _damage;

        [Space]
        [SerializeField]
        private GunInfo _gunInfo;

        #endregion



        #region Properties

        public string Id => name.Replace(PrefixName, string.Empty);


        public CharacterModel CharacterModel => _characterModel;
        public int Health => _health;
        public float Speed => _speed;
        public int Damage => _damage;


        public GunInfo GunInfo => _gunInfo;

        #endregion
    }
}
