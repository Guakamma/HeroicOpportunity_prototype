using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif


namespace HeroicOpportunity.Paths
{
    public static class Common
    {
        public const string AssetsResourcesRoot = "Assets/Resources/";
        public const string PrefabsRoot = "Prefabs/";


        #if UNITY_EDITOR

        public static string GetResourcesAssetPath(Object obj)
        {
            string withExtension = AssetDatabase.GetAssetPath(obj)
                .Replace(AssetsResourcesRoot, "");
            return Path.ChangeExtension(withExtension, null);
        }


        public static string EnsurePathForwardSlash(string str) => str.Replace(Path.DirectorySeparatorChar, '/');

        #endif
    }
}
