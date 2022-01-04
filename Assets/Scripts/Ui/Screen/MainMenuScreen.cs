using System;
using HeroicOpportunity.Game;
using HeroicOpportunity.Services;
using Services;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace HeroicOpportunity.Ui
{
    public class MainMenuScreen : Screen
    {
        #region Fields

        [SerializeField] [Required]
        private Button _playButton;

        [SerializeField]
        [Required]
        private TextMeshProUGUI _levelLabel;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            _playButton.OnClickAsObservable()
                .Subscribe(_ => PlayButton_OnClick())
                .AddTo(this);
        }


        private void OnEnable()
        {
            _levelLabel.text = "Level " + ServicesHub.Level.LevelNumber;
        }

        #endregion



        #region Private methods

        private void PlayButton_OnClick()
        {
            GameManager.Instance.SetGameState(GameStateType.InGame);
        }

        #endregion
    }
}
