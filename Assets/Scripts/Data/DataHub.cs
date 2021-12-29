using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Data.Enemies;
using HeroicOpportunity.Data.Heroes;
using HeroicOpportunity.Data.Levels;
using UnityEngine;


namespace HeroicOpportunity.Data
{
    public class DataHub : Singleton<DataHub>
    {
        #region Fields

        private static LevelsData levelsData;
        private static HeroesData heroesData;
        private static EnemiesData enemiesData;
        private static AbilitiesData abilitiesData;

        #endregion



        #region Properties

        public static LevelsData Levels => Instance.GetLazy(ref levelsData, "Data/Levels/Data_Levels");
        public static HeroesData Heroes => Instance.GetLazy(ref heroesData, "Data/Heroes/Data_Heroes");
        public static EnemiesData Enemies => Instance.GetLazy(ref enemiesData, "Data/Enemies/Data_Enemies");
        public static AbilitiesData Abilities => Instance.GetLazy(ref abilitiesData, "Data/Abilities/Data_Abilities");

        #endregion



        #region Private methods

        private T GetLazy<T>(ref T backingStorage, string resourcePath) where T : ScriptableObject
        {
            if (backingStorage == null)
            {
                backingStorage = Resources.Load<T>(resourcePath);
            }

            return backingStorage;
        }


        #if UNITY_EDITOR

        [CustomSetup(Priority = -1000)]
        private static void CustomSetup()
        {
            FreeInstance();
        }

        #endif

        #endregion
    }
}
