using Game;
using HeroicOpportunity.Ui;
using Sirenix.OdinInspector;
using UnityEngine;


namespace HeroicOpportunity.Game
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public const string StartupScene = "Startup";

        [SerializeField] [Required]
        private Transform _gameRoot;

        [SerializeField] [Required]
        private Camera _mainCamera;

        [SerializeField] [Required]
        private UiManager _uiManager;

        private GameStateController _stateController;


        public Transform GameRoot => _gameRoot;
        public Camera MainCamera => _mainCamera;
        public UiManager UIManager => _uiManager;
        public GameStateType CurrentState => _stateController.CurrentState;
        
        
        public void SetGameState(GameStateType gameStateType) => _stateController.SetState(gameStateType);
        

        private void Awake()
        {
            Application.targetFrameRate = 60;
            UnityEngine.Input.multiTouchEnabled = false;

            _uiManager.Initialize();
            _stateController = new GameStateController();
        }

        private void Start()
        {
            SetGameState(GameStateType.MainMenu);
        }
    }
}
