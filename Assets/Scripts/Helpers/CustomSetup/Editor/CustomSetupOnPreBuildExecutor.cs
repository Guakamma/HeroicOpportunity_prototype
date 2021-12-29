using UnityEditor.Build;
using UnityEditor.Build.Reporting;


public class CustomSetupOnPreBuildExecutor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;


    public void OnPreprocessBuild(BuildReport report)
    {
        CustomSetupExecutor.Execute();
    }
}
