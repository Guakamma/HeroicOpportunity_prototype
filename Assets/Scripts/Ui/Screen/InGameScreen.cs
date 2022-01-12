using System.Collections.Generic;
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
        [SerializeField] [Required]
        private Button _restartButton;
        
        [SerializeField] [Required]
        private Button _skipLevelButton;

        [SerializeField] [Required]
        private RectTransform _abilitiesRoot;

        [SerializeField] [Required]
        private RectTransform _abilityComboRoot;
        
        [SerializeField] [Required]
        private GameObject _comboArrow;

        private List<AbilityCardInGame> _abilities;


        private void Awake()
        {
            _abilities = new List<AbilityCardInGame>();
            
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
                _abilities.Add(abilityCard);
            }

            gameObject.AddComponent<AbilityCombo>().Initialize(_abilityComboRoot, _comboArrow);
        }

        public override void Show()
        {
            base.Show();
            foreach (AbilityCardInGame ability in _abilities)
            {
                ability.Reload();
            }
        }

        private void SkipLevel()
        {
            ServicesHub.Level.ActiveLevel.LevelWin();
        }

        private void RestartButton_OnClick()
        {
            GameManager.Instance.SetGameState(GameStateType.Restart);
        }
    }
}
