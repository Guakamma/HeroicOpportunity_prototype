using HeroicOpportunity.Game;
using HeroicOpportunity.Ui;

namespace Game.State
{
    public class InGameState : IState
    {
        #region IState

        public void OnEnter()
        {
            GameManager.Instance.UIManager.ShowScreen(ScreenType.InGame);
        }


        public void OnExit()
        {
        }

        #endregion
    }
}
