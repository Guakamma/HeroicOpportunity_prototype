using System.Collections.Generic;
using System.Linq;
using Data.Enemies;
using HeroicOpportunity.Paths;
using Sirenix.OdinInspector;
using UnityEngine;


#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif


namespace HeroicOpportunity.Data.Enemies
{
    [CreateAssetMenu(fileName = "Data_Enemies", menuName = "Data/Enemies/Enemies Data")]
    public class EnemiesData : SerializedScriptableObject
    {
        #region Fields

        [SerializeField] [ReadOnly]
        private Dictionary<string, string> _enemiesInfoPaths;

        #endregion



        #region Properties

        public Dictionary<string, string> EnemiesInfoPaths => _enemiesInfoPaths;

        #endregion



        #region Public methods

        public string[] GetAllIds()
        {
            return _enemiesInfoPaths.Keys.ToArray();
        }

        #endregion



        #if UNITY_EDITOR

        [CustomSetup]
        private static void CustomSetup()
        {
            EnemiesData target = DataHub.Enemies;

            string targetParentFolder = Path.GetDirectoryName(Common.GetResourcesAssetPath(target));
            target._enemiesInfoPaths = Resources.LoadAll<EnemyInfo>(targetParentFolder)
            .ToDictionary(i => i.Id, Common.GetResourcesAssetPath);

            EditorUtility.SetDirty(target);
        }

        #endif
    }
}
