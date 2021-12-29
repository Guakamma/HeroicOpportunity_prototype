
public class Singleton<T> where T : Singleton<T>, new()
{
    private static T _instance = null;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            _instance = new T();
            _instance.Init();
        }
        return _instance;
    }

    public static T Instance
    {
        get
        {
            return GetInstance();
        }
    }

    public static bool IsAlive()
    {
        return _instance != null;
    }

    public static void FreeInstance()
    {
        if (_instance != null)
        {
            _instance.DeInit();
            _instance = null;
        }
    }

    protected virtual void Init()
    {
    }

    protected virtual void DeInit()
    {
    }
}
