using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Game;
using Services;
using Sirenix.OdinInspector;
using Ui.Abilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace HeroicOpportunity.Ui
{
    public class InGameScreen : Screen
    {
        #region Fields

        [SerializeField] [Required]
        private Button _restartButton;
        
        [SerializeField] [Required]
        private Button _skipLevelButton;

        [SerializeField] [Required]
        private RectTransform _abilitiesRoot;

        [SerializeField] [Required]
        private RectTransform _abilityComboRoot;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            _restartButton.OnClickAsObservable()
                .Subscribe(_ => RestartButton_OnClick())
                .AddTo(this);
            
            _skipLevelButton.OnClickAsObservable()
                .Subscribe(_ => SkipLevel())
                .AddTo(this);

            AbilityCardInGame abilityCardPrefab = Resources.Load<AbilityCardInGame>(Paths.Ui.AbilitiesCardInGame);
            foreach (AbilityInfo abilityInfo in ServicesHub.Abilities.GetAllAbilityInfos())
            {
                AbilityCardInGame abilityCard = Instantiate(abilityCardPrefab, _abilitiesRoot);
                abilityCard.Initialize(abilityInfo);
            }

            gameObject.AddComponent<AbilityCombo>().Initialize(_abilityComboRoot);
        }

        private void SkipLevel()
        {
            ServicesHub.Level.ActiveLevel.LevelWin();
        }

        #endregion



        #region Private methods

        private void RestartButton_OnClick()
        {
            GameManager.Instance.SetGameState(GameStateType.Restart);
        }

        #endregion
    }
}
