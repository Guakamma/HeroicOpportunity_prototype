using Character;
using HeroicOpportunity.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Heroes
{
    [CreateAssetMenu(fileName = PrefixName + "name", menuName = "Data/Heroes/Hero Info")]
    public class HeroInfo : ScriptableObject
    {
        private const string PrefixName = "Data_Heroes_Infos_";

        [SerializeField] [Required]
        private CharacterModel _characterModel;
        
        [SerializeField] [Min(0)]
        private int _health;
        
        [SerializeField] [Min(0.0f)]
        private float _speed;
        
        [SerializeField] [Min(0)]
        private int _damage;

        [SerializeField] [Required]
        private float _comboProtectDuration;

        [Header("Gun Info")]
        [SerializeField]
        private GunInfo _gunInfo;



        public string Id => name.Replace(PrefixName, string.Empty);

        public CharacterModel CharacterModel => _characterModel;
        public int Health => _health;
        public float Speed => _speed;
        public int Damage => _damage;
        public float ComboProtectDuration => _comboProtectDuration;

        public GunInfo GunInfo => _gunInfo;
    }
}
