using HeroicOpportunity.Game;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace HeroicOpportunity.Character
{
    public class HealthBar : MonoBehaviour
    {
        #region Fields

        [SerializeField] [Required]
        private Slider _scrollbar;

        #endregion



        #region Public methods

        public void Initialize(ICharacter character, int maxHealth)
        {
            transform.rotation = Quaternion.LookRotation(GameManager.Instance.MainCamera.transform.forward);
            SetScrollValue(character.Health, maxHealth);

            character.ObserveEveryValueChanged(c => c.Health)
                .Subscribe(h => SetScrollValue(h, maxHealth))
                .AddTo(this);
        }

        #endregion



        #region Private methods

        private void SetScrollValue(int health, int maxHealth)
        {
            _scrollbar.value = (float)health / maxHealth;
        }

        #endregion
    }
}
