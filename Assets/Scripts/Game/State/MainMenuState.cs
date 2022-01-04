using HeroicOpportunity.Services;
using HeroicOpportunity.Ui;
using Services;
using UnityEngine;


namespace HeroicOpportunity.Game
{
    public class MainMenuState : IState
    {
        #region IState

        public void OnEnter()
        {
            ServicesHub.Level.CrateLevel();
            ServicesHub.Hero.CreateHero(GameManager.Instance.GameRoot, Vector3.zero);
            GameManager.Instance.UIManager.ShowScreen(ScreenType.MainMenu);
        }


        public void OnExit() { }

        #endregion
    }
}
