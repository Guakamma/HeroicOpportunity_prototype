using System.Collections.Generic;
using System.IO;
using System.Linq;
using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Paths;
using Services.Abilities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Data.Abilities
{
    [CreateAssetMenu(fileName = "Data_Abilities", menuName = "Data/Abilities/Abilities Data")]
    public class AbilitiesData : SerializedScriptableObject
    {
        [SerializeField] [ReadOnly]
        private Dictionary<AbilityType, string> _abilitiesInfoPaths;

        public Dictionary<AbilityType, string> AbilitiesInfoPaths => _abilitiesInfoPaths;


        #if UNITY_EDITOR

        [CustomSetup]
        private static void CustomSetup()
        {
            AbilitiesData target = DataHub.Abilities;

            string targetParentFolder = Path.GetDirectoryName(Common.GetResourcesAssetPath(target));
            target._abilitiesInfoPaths = Resources.LoadAll<AbilityInfo>(targetParentFolder)
                .ToDictionary(i => i.Type, Common.GetResourcesAssetPath);

            EditorUtility.SetDirty(target);
        }

        #endif
    }
}
