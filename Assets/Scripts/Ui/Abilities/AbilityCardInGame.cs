using DG.Tweening;
using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Ui;
using Services;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Abilities
{
    public class AbilityCardInGame : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private AbilityCard _abilityCard;

        [SerializeField]
        [Required]
        private Button _button;

        private AbilityInfo _abilityInfo;


        public void Initialize(AbilityInfo abilityInfo)
        {
            _abilityInfo = abilityInfo;

            _abilityCard.Initialize(_abilityInfo);
            Reload();

            _button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    ServicesHub.Events.Ability.Damage(_abilityInfo);
                    Reload();
                })
                .AddTo(this);
        }

        private void Reload()
        {
            _abilityCard.SetFade(1.0f);
            _button.interactable = false;
            _abilityCard.Fade.DOFillAmount(0.0f, _abilityInfo.ReloadTime)
                .SetLink(gameObject)
                .OnComplete(() => _button.interactable = true);

            _abilityCard.CooldownTimer.gameObject.SetActive(true);
            _abilityCard.CooldownTimer.DOCounter((int)_abilityInfo.ReloadTime, 0, _abilityInfo.ReloadTime)
                .SetLink(gameObject)
                .OnComplete(() => _abilityCard.CooldownTimer.gameObject.SetActive(false));
        }
    }
}
