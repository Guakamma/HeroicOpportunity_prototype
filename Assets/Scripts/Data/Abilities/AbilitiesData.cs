using System.Collections.Generic;
using System.Linq;
using HeroicOpportunity.Paths;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace HeroicOpportunity.Data.Abilities
{
    [CreateAssetMenu(fileName = "Data_Abilities", menuName = "Data/Abilities/Abilities Data")]
    public class AbilitiesData : SerializedScriptableObject
    {
        #region Fields

        [SerializeField] [ReadOnly]
        private Dictionary<string, string> _abilitiesInfoPaths;

        #endregion



        #region Properties

        public Dictionary<string, string> AbilitiesInfoPaths => _abilitiesInfoPaths;
        public string[] AllIds => _abilitiesInfoPaths.Keys.ToArray();

        #endregion



        #if UNITY_EDITOR

        [CustomSetup]
        private static void CustomSetup()
        {
            AbilitiesData target = DataHub.Abilities;

            string targetParentFolder = Path.GetDirectoryName(Common.GetResourcesAssetPath(target));
            target._abilitiesInfoPaths = Resources.LoadAll<AbilityInfo>(targetParentFolder)
                .ToDictionary(i => i.Id, Common.GetResourcesAssetPath);

            EditorUtility.SetDirty(target);
        }

        #endif
    }
}
