using UnityEngine;

namespace Services.Prefs
{
    public class PrefsServiceImpl : IPrefsService
    {
        private static void Save() => PlayerPrefs.Save();


        public bool HasKey(string key) => PlayerPrefs.HasKey(key);
        public void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);


        public void SetString(string key, string value, bool isSaveImmediately = false)
        {
            PlayerPrefs.SetString(key, value);

            if (isSaveImmediately)
            {
                Save();
            }
        }


        public string GetString(string key) => GetString(key, string.Empty);
        public string GetString(string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);


        public void SetInt(string key, int value, bool isSaveImmediately = false)
        {
            PlayerPrefs.SetInt(key, value);

            if (isSaveImmediately)
            {
                Save();
            }
        }


        public int GetInt(string key) => GetInt(key, 0);
        public int GetInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
    }
}
