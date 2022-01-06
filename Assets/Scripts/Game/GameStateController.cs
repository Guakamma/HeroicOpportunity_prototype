using System;
using System.Collections.Generic;
using UniRx;


namespace HeroicOpportunity.Game
{
    public class GameStateController
    {
        private static readonly Subject<GameStateType> StateChanged = new Subject<GameStateType>();
        public static IObservable<GameStateType> OnStateChanged => StateChanged.AsObservable();

        private readonly Dictionary<GameStateType, IState> _stateByType;
        private IState _previousState;

        public GameStateType CurrentState { get; private set; }


        public GameStateController()
        {
            _stateByType = new Dictionary<GameStateType, IState>
            {
                { GameStateType.MainMenu, new MainMenuState() },
                { GameStateType.InGame, new InGameState() },
                { GameStateType.Result, new ResultState() },
                { GameStateType.Restart, new RestartState() }
            };
        }


        public void SetState(GameStateType gameStateType)
        {
            if (_previousState != null)
            {
                _previousState.OnExit();
            }

            StateChanged.OnNext(gameStateType);
            CurrentState = gameStateType;

            _previousState = _stateByType[gameStateType];
            _previousState.OnEnter();
        }
    }
}
