using Services.Abilities;
using Sirenix.OdinInspector;
using UnityEngine;


namespace HeroicOpportunity.Data.Abilities
{
    [CreateAssetMenu(fileName = PrefixName + "name", menuName = "Data/Abilities/Ability Info")]
    public class AbilityInfo : ScriptableObject
    {
        #region Fields

        private const string PrefixName = "Data_Abilities_Infos_";
        
        
        [SerializeField] [Required]
        private AbilityType _type;
        
        [SerializeField] [Required]
        private Sprite _icon;

        [SerializeField] [Min(0.0f)]
        private float _reloadTime;

        [SerializeField] [Min(0)]
        private int _damage;

        #endregion



        #region Properties

        public string Id => name.Replace(PrefixName, string.Empty);


        public AbilityType Type => _type;
        public Sprite Icon => _icon;
        public float ReloadTime => _reloadTime;
        public int Damage => _damage;

        #endregion
    }
}
