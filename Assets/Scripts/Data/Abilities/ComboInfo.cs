using HeroicOpportunity.Data;
using Services.Abilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Abilities
{
    [CreateAssetMenu(fileName = PrefixName + "name", menuName = "Data/Combo/Combo Info")]
    public class ComboInfo : ScriptableObject
    {
        private const string PrefixName = "Data_Ability_Combo_Info_";

        [SerializeField]
        private AbilityType[] _abilityCombo;
        
        [SerializeField]
        [MinValue(0)]
        private int _abilityComboDamage;

        [SerializeField]
        private bool _isRandomActive;


        public AbilityType[] AbilityComboSchedule => _abilityCombo;
        public int AbilityComboDamage => _abilityComboDamage;
        public bool IsRandomActive => _isRandomActive;
        
        public string Id => name.Replace(PrefixName, string.Empty);
    }
}