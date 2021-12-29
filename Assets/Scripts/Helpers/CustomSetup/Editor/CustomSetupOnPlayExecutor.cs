using System;
using System.Linq;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public static class CustomSetupOnPlayExecutor
{
    static CustomSetupOnPlayExecutor()
    {
        EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;

        Undo.postprocessModifications += PostprocessModifications;
    }


    static void EditorApplication_PlayModeStateChanged(PlayModeStateChange change)
    {
        switch (change)
        {
            case PlayModeStateChange.ExitingEditMode:
                try
                {
                    CustomSetupExecutor.Execute();
                }
                catch (Exception e)
                {
                    Exception exceptionToLog = e.InnerException != null ? e.InnerException : e;

                    Debug.LogError("Exception occured during CustomSetup:\n" +
                                         $"{(exceptionToLog.Message)}\n" +
                                         $"Stack trace:\n{exceptionToLog.StackTrace}");
                    EditorApplication.isPlaying = false;
                }

                break;
            default:
                break;
        }
    }

    private static UndoPropertyModification[] PostprocessModifications(UndoPropertyModification[] modifications)
    {
        var hasResourceAsset = HasResourceAsset(Selection.assetGUIDs);
        if (hasResourceAsset)
        {
            AssetDatabase.SaveAssets();
        }

        return modifications;
    }

    private static bool HasResourceAsset(string[] assetsGuids)
    {
        return assetsGuids.Select(AssetDatabase.GUIDToAssetPath).Any(IsResourcesAssets);
    }

    private static bool IsResourcesAssets(string path)
    {
        return path.Contains("Assets/Resources");
    }
}