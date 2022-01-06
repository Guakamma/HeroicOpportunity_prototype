using HeroicOpportunity.Paths;
using Sirenix.OdinInspector;
using System.Linq;
using Data.Levels;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif


namespace HeroicOpportunity.Data.Levels
{
    [CreateAssetMenu(fileName = "Data_Levels", menuName = "Data/Levels/Levels Data")]
    public class LevelsData : ScriptableObject
    {
        #region Fields

        [SerializeField] [ReadOnly]
        private string[] _levelPaths;

        #endregion



        #region Properties

        public string[] LevelPaths => _levelPaths;

        #endregion



        #if UNITY_EDITOR

        [CustomSetup]
        private static void CustomSetup()
        {
            LevelsData target = DataHub.Levels;

            string targetParentFolder = Path.GetDirectoryName(Common.GetResourcesAssetPath(target));
            target._levelPaths = Resources.LoadAll<LevelInfo>(targetParentFolder)
                .Select(Common.GetResourcesAssetPath)
                .ToArray();

            EditorUtility.SetDirty(target);
        }

        #endif
    }
}
