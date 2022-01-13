using Character;
using HeroicOpportunity.Character;
using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Enemies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Enemies
{
    [CreateAssetMenu(fileName = PrefixName + "name", menuName = "Data/Enemies/Enemy Info")]
    public class EnemyInfo : ScriptableObject
    {
        private const string PrefixName = "Data_Enemies_Info_";

        [SerializeField]
        private EnemyType _enemyType;

        [SerializeField]
        [Required]
        private CharacterModel _characterModel;

        [SerializeField]
        [MinValue(0.0f)]
        private float _heroDistance;
        
        [SerializeField]
        [MinValue(0.0f)]
        private float _speed;

        [SerializeField]
        [MinValue(0.0f)]
        private int _damage;

        [SerializeField]
        [MinValue(0)]
        private int _health;

        [SerializeField]
        private GunInfo _gunInfo;
        
        
        public string Id => name.Replace(PrefixName, string.Empty);
        public EnemyType EnemyType => _enemyType;
        public CharacterModel CharacterModel => _characterModel;
        public float HeroDistance => _heroDistance;
        public float Speed => _speed;
        public int Damage => _damage;
        public int Health => _health;
        public GunInfo GunInfo => _gunInfo;
        public bool IsBoss => _enemyType == EnemyType.Boss;
    }
}
