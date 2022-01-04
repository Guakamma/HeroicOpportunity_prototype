using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Abilities;
using HeroicOpportunity.Paths;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace HeroicOpportunity.Data.Abilities
{
    [CreateAssetMenu(fileName = "Data_Combo", menuName = "Data/Combo/Combo Data")]
    public class ComboData : SerializedScriptableObject
    {
        #region Fields

        [SerializeField] [ReadOnly]
        private Dictionary<string, string> _comboInfoPaths;

        #endregion



        #region Properties

        public Dictionary<string, string> ComboInfoPaths => _comboInfoPaths;
        public string[] AllIds => _comboInfoPaths.Keys.ToArray();

        #endregion



#if UNITY_EDITOR

        [CustomSetup]
        private static void CustomSetup()
        {
            ComboData target = DataHub.Combo;

             string targetParentFolder = Path.GetDirectoryName(Common.GetResourcesAssetPath(target));
             target._comboInfoPaths = Resources.LoadAll<ComboInfo>(targetParentFolder)
                 .ToDictionary(i => i.Id, Common.GetResourcesAssetPath);
            
             EditorUtility.SetDirty(target);
        }

#endif
    }
}