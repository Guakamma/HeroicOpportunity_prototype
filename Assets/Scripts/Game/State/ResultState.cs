using HeroicOpportunity.Ui;


namespace HeroicOpportunity.Game
{
    public class ResultState : IState
    {
        #region IState

        public void OnEnter()
        {
            GameManager.Instance.UIManager.ShowScreen(ScreenType.Result);
        }


        public void OnExit() { }

        #endregion
    }
}
