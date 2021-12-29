using HeroicOpportunity.Game;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace HeroicOpportunity.Ui
{
    public class ResultScreen : Screen
    {
        #region Fields

        [SerializeField] [Required]
        private Button _mainMenuButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            _mainMenuButton.OnClickAsObservable()
                .Subscribe(_ => MainMenuButton_OnClick())
                .AddTo(this);
        }

        #endregion



        #region Private methods

        private void MainMenuButton_OnClick()
        {
            GameManager.Instance.SetGameState(GameStateType.MainMenu);
        }

        #endregion
    }
}
