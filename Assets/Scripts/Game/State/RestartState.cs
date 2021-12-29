using HeroicOpportunity.Services;
using UnityEngine;


namespace HeroicOpportunity.Game
{
    public class RestartState : IState
    {
        #region IState

        public void OnEnter()
        {
            GameManager.Instance.SetGameState(GameStateType.InGame);
        }


        public void OnExit()
        {
            ServicesHub.Level.CrateLevel();
            ServicesHub.Hero.CreateHero(GameManager.Instance.GameRoot, Vector3.zero);
        }

        #endregion
    }
}
