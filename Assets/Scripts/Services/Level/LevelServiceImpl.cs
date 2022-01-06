using System.Collections.Generic;
using Data.Levels;
using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Levels;
using HeroicOpportunity.Game;
using HeroicOpportunity.Services.Level;
using HeroicOpportunity.Services.Prefs;
using Input;
using Level;
using UnityEngine;

namespace Services.Level
{
    public class LevelServiceImpl : ILevelService
    {
        #region Fields

        private readonly IPrefsService _prefsService;
        private readonly List<LevelInfo> _levels;

        #endregion



        #region Properties

        private LevelsData LevelsData => DataHub.Levels;

        #endregion



        #region Class lifecycle

        public LevelServiceImpl(IPrefsService prefsService)
        {
            _prefsService = prefsService;

            _levels = new List<LevelInfo>();
            foreach (var p in LevelsData.LevelPaths)
            {
                LevelInfo levelInfo = Resources.Load<LevelInfo>(p);
                _levels.Add(levelInfo);
            }
        }

        #endregion



        #region Private methods

        private LevelInfo GetActualLevelInfo()
        {
            int index = LevelNumber - 1;
            return _levels[index % _levels.Count];
        }

        #endregion



        #region ILevelService

        public int LevelNumber
        {
            get => _prefsService.GetInt(PrefsKeys.Level.Number, 1);
            private set => _prefsService.SetInt(PrefsKeys.Level.Number, value);
        }

        public LevelController ActiveLevel { get; private set; }
        public void CrateLevel()
        {
            DisposeActiveLevel();

            ActiveLevel = new GameObject(nameof(LevelController))
                .AddComponent<LevelController>();
            ActiveLevel.transform.SetParent(GameManager.Instance.GameRoot);
            ActiveLevel.Initialize(GetActualLevelInfo());

            ActiveLevel.gameObject.AddComponent<SwipeHandler>();
        }


        public void DisposeActiveLevel()
        {
            if (ActiveLevel == null)
            {
                return;
            }

            Object.Destroy(ActiveLevel.gameObject);
            ActiveLevel = null;
        }


        public void IncrementLevelNumber()
        {
            LevelNumber++;
        }

        #endregion
    }
}
