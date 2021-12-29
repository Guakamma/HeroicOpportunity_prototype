using System.Collections.Generic;
using System.Linq;
using HeroicOpportunity.Paths;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif


namespace HeroicOpportunity.Data.Heroes
{
    [CreateAssetMenu(fileName = "Data_Heroes", menuName = "Data/Heroes/Heroes Data")]
    public class HeroesData : SerializedScriptableObject
    {
        #region Fields

        [SerializeField] [ReadOnly]
        private Dictionary<string, string> _heroInfoPaths;
        [SerializeField] [ValueDropdown(nameof(AllNames))]
        private string _defaultNameHero;

        #endregion



        #region Properties

        public Dictionary<string, string> HeroInfoPaths => _heroInfoPaths;
        public string[] AllNames => _heroInfoPaths.Keys.ToArray();
        public string DefaultNameHero => _defaultNameHero;

        #endregion



        #if UNITY_EDITOR

        [CustomSetup]
        private static void CustomSetup()
        {
            HeroesData target = DataHub.Heroes;

            string targetParentFolder = Path.GetDirectoryName(Common.GetResourcesAssetPath(target));
            target._heroInfoPaths = Resources.LoadAll<HeroInfo>(targetParentFolder)
                .ToDictionary(i => i.Id, Common.GetResourcesAssetPath);

            EditorUtility.SetDirty(target);
        }

        #endif
    }
}
