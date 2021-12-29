using HeroicOpportunity.Game;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class EditorSceneSwitcher
{
    private const string PlayFromFirstMenuStr = "Tools/Run from Startup Scene";


    private static bool PlayFromFirstScene
    {
        get => EditorPrefs.GetBool(PlayFromFirstMenuStr, true);
        set => EditorPrefs.SetBool(PlayFromFirstMenuStr, value);
    }


    [MenuItem(PlayFromFirstMenuStr, false, 150)]
    private static void PlayFromFirstSceneCheckMenu()
    {
        PlayFromFirstScene = !PlayFromFirstScene;
        Menu.SetChecked(PlayFromFirstMenuStr, PlayFromFirstScene);

        ShowNotifyOrLog(PlayFromFirstScene ? "Play from startup scene" : "Play from current scene");
    }


    // The menu won't be gray out, we use this validate method for update check state
    [MenuItem(PlayFromFirstMenuStr, true)]
    public static bool PlayFromFirstSceneCheckMenuValidate()
    {
        Menu.SetChecked(PlayFromFirstMenuStr, PlayFromFirstScene);
        return true;
    }


    // This method is called before any Awake. It's the perfect callback for this feature
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadFirstSceneAtGameBegins()
    {
        if (!PlayFromFirstScene || SceneManager.GetActiveScene().name == GameManager.StartupScene)
            return;


        foreach (GameObject go in Object.FindObjectsOfType<GameObject>())
        {
            go.SetActive(false);
        }

        SceneManager.LoadScene(GameManager.StartupScene);
    }


    static void ShowNotifyOrLog(string msg)
    {
        if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
            EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
        else
            Debug.Log(msg); // When there's no scene view opened, we just print a log
    }
}
