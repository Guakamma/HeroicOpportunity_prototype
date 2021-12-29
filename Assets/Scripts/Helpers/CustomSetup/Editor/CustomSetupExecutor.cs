using System;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using UnityEditor;


public static class CustomSetupExecutor
{
    public static void Execute()
    {
        Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var methodsToRun = allAssemblies
            .SelectMany(a => a.GetTypes())
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                .Where(m => m.GetCustomAttributes(typeof(CustomSetupAttribute), false).Length > 0)
            .OrderBy(m =>
                m.GetCustomAttribute<CustomSetupAttribute>(false).Priority)
            .ToArray();

        if (methodsToRun.Length > 0)
        {
            AssetDatabase.StartAssetEditing();

            try
            {
                int i = 1;
                foreach (var method in methodsToRun)
                {
                    EditorUtility.DisplayProgressBar("Custom Setup running...",
                        $"[{i}/{methodsToRun.Length}] {method.DeclaringType.Name}.{method.Name}",
                        (float)i / methodsToRun.Length);
                    method.Invoke(null, null);

                    ++i;
                }
            }
            catch (Exception ex)
            {
                var capture = ExceptionDispatchInfo.Capture(ex);
                capture.Throw();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.ClearProgressBar();
            }

        }
    }
}
