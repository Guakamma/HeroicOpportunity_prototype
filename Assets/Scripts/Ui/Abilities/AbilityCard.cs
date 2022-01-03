using HeroicOpportunity.Data.Abilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


namespace HeroicOpportunity.Ui
{
    public class AbilityCard : MonoBehaviour
    {
        #region Fiedld

        [SerializeField]
        [Required]
        protected Image _icon;

        [SerializeField]
        [Required]
        protected Image _fade;
        
        [SerializeField]
        protected Text _cooldownTimer;

        private AbilityInfo _abilityInfo;

        #endregion



        #region Properties

        public string Id => _abilityInfo.Id;
        public Image Fade => _fade;
        public Text CooldownTimer => _cooldownTimer;
        public bool IsFade => Fade.fillAmount > 0.0f;

        #endregion



        #region Public methods

        public void Initialize(AbilityInfo abilityInfo)
        {
            _abilityInfo = abilityInfo;

            _icon.sprite = _abilityInfo.Icon;
            _fade.sprite = _abilityInfo.Icon;
            
            if (_cooldownTimer != null)
                _cooldownTimer.gameObject.SetActive(false);
        }


        public void SetFade(float value)
        {
            _fade.fillAmount = value;
        }

        #endregion
    }
}
