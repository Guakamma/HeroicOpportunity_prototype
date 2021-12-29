using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace HeroicOpportunity.Ui
{
    public class UiManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] [Required]
        private RectTransform _screensRoot;

        private Dictionary<ScreenType, Screen> _screens;
        private Screen _activeScreen;

        #endregion



        #region Public methods

        public void Initialize()
        {
            _screens = new Dictionary<ScreenType, Screen>();
        }


        public void ShowScreen(ScreenType screenType)
        {
            if (_activeScreen != null)
            {
                _activeScreen.Hide();
            }

            _activeScreen = GetScreen(screenType);
            _activeScreen.Show();
        }

        #endregion



        #region Private methods

        private Screen GetScreen(ScreenType screenType)
        {
            if (_screens.ContainsKey(screenType))
            {
                return _screens[screenType];
            }

            Screen prefab = Resources.Load<Screen>(Paths.Ui.GetScreenPath(screenType));
            Screen screen = Instantiate(prefab, _screensRoot);
            _screens.Add(screenType, screen);
            return screen;
        }

        #endregion
    }
}
