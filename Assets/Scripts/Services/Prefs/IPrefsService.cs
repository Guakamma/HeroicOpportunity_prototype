namespace Services.Prefs
{
    public interface IPrefsService
    {
        bool HasKey(string key);
        void DeleteKey(string key);


        void SetString(string key, string value, bool isSaveImmediately = false);
        string GetString(string key);
        string GetString(string key, string defaultValue);


        void SetInt(string key, int value, bool isSaveImmediately = false);
        int GetInt(string key);
        int GetInt(string key, int defaultValue);
    }
}
