using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class JenkinsBuildPipeline : MonoBehaviour
{
    public static void PerformBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = FindEnabledEditorScenes();
        buildPlayerOptions.locationPathName = "iOSBuild";
        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        //XCodePostProcess.UpdatePlist(target, );
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded:!!! " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    private static string[] FindEnabledEditorScenes(){

        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes){
            if (scene.enabled){
                EditorScenes.Add(scene.path);
            }
        }
        return EditorScenes.ToArray();
    }
}
