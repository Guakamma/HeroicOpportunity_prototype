using System;
using HeroicOpportunity.Character;
using Sirenix.OdinInspector;
using UnityEngine;


namespace HeroicOpportunity.Data.Enemies
{
    [CreateAssetMenu(fileName = PrefixName + "name", menuName = "Data/Enemies/Enemy Info")]
    public class EnemyInfo : ScriptableObject
    {
        #region Fields

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
        private int _damage;

        [SerializeField]
        [MinValue(0)]
        private int _health;

        [SerializeField]
        private GunInfo _gunInfo;

        [SerializeField]
        [ShowIf(nameof(IsBoss))]
        [ValueDropdown(nameof(AllAbilityIds))]
        private string[] _abilityCombo;

        [SerializeField]
        [ShowIf(nameof(IsBoss))]
        [MinValue(0)]
        private int _abilityComboDamage;

        #endregion



        #region Properties

        public string Id => name.Replace(PrefixName, string.Empty);
        public EnemyType EnemyType => _enemyType;
        public CharacterModel CharacterModel => _characterModel;
        public float HeroDistance => _heroDistance;


        public int Damage => _damage;
        public int Health => _health;
        public GunInfo GunInfo => _gunInfo;
        public bool IsBoss => _enemyType == EnemyType.Boss;
        public string[] AbilityCombo => IsBoss ? _abilityCombo : Array.Empty<string>();
        public int AbilityComboDamage => IsBoss ? _abilityComboDamage : 0;

        #endregion



        #region Private methods

        private string[] AllAbilityIds()
        {
            return DataHub.Abilities.AllIds;
        }

        #endregion
    }
}
