using System.Collections;
using DG.Tweening;
using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Game;
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
        private Coroutine _reloadTimer;


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

        public void Reload()
        {
            _abilityCard.Fade.DOKill();
            
            _abilityCard.SetFade(1.0f);
            _button.interactable = false;
            _abilityCard.Fade.DOFillAmount(0.0f, _abilityInfo.ReloadTime)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .OnComplete(() => _button.interactable = true);

            if (_reloadTimer != null)
            {
                StopCoroutine(_reloadTimer);
                _reloadTimer = null;
            }
            
            if (!gameObject.activeSelf)
                gameObject.SetActive(true); 
            
            _reloadTimer = StartCoroutine(ReloadTimerRoutine(_abilityInfo.ReloadTime, 0));
        }

        private IEnumerator ReloadTimerRoutine(float startValue, float endValue)
        {
            var waiter = new WaitForSecondsRealtime(0.1f);
            _abilityCard.CooldownTimer.gameObject.SetActive(true);
            
            while (startValue > endValue)
            {
                _abilityCard.CooldownTimer.text = $"{startValue:f1}";
                startValue -= 0.1f;
                yield return waiter;
            }

            _abilityCard.CooldownTimer.gameObject.SetActive(false);
            _reloadTimer = null;
        }
    }
}
