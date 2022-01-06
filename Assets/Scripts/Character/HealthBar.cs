using HeroicOpportunity.Character;
using HeroicOpportunity.Game;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class HealthBar : MonoBehaviour
    {
        #region Fields

        [SerializeField] [Required]
        private Slider _scrollbar;
        
        [SerializeField] [Required]
        private TextMeshProUGUI _healthCounter;
        
        [SerializeField] [Required]
        private RectTransform _damageIndicatorPoint;

        [Header("Damage Indicators")]
        [SerializeField]
        private float _heightPosition = 1f;
        [SerializeField]
        private float _animationDuration = 2f;

        private DamageIndicatorSpawner _indicatorSpawner;
        private int _savedHealth = 0;

        #endregion



        #region Public methods

        public void Initialize(ICharacter character, int maxHealth)
        {
            transform.rotation = Quaternion.LookRotation(GameManager.Instance.MainCamera.transform.forward);
            SetScrollValue(character.Health, maxHealth);
            SetHealthCounter(character.Health);
            _savedHealth = character.Health;

            character.ObserveEveryValueChanged(c => c.Health)
                .Subscribe(health =>
                {
                    SetScrollValue(health, maxHealth);
                    SetHealthCounter(health);

                    if (_savedHealth < character.Health)
                        _savedHealth = character.Health;
                    else
                        PlayGetDamageEffect(character.Health);
                })
                .AddTo(this);

            _indicatorSpawner = new DamageIndicatorSpawner(_damageIndicatorPoint, _heightPosition, _animationDuration, color: Color.black);
        }

        #endregion



        #region Private methods

        private void SetScrollValue(int health, int maxHealth)
        {
            _scrollbar.value = (float)health / maxHealth;
        }
        
        private void SetHealthCounter(int health)
        {
            _healthCounter.text = Mathf.Max(0, health).ToString();
        }

        private void PlayGetDamageEffect(int currentHealth)
        {
            int damage = _savedHealth - currentHealth;
            _savedHealth = currentHealth;
            
            if (damage > 0)
                _indicatorSpawner.Show(damage);
        }

        #endregion
    }
}
