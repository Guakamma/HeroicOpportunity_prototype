using HeroicOpportunity.Ui;


namespace HeroicOpportunity.Game
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
