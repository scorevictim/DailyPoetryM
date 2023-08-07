namespace DailyPoetryM.Services;

public class PreferenceStorage : IPreferenceStorage
{
    public int Get(string key, int defaultValue)
    {
        return Preferences.Get(key, defaultValue);
    }

    public string Get(string key, string defaultValue)
    {
        return Preferences.Get(key, defaultValue);
    }

    public void Set(string key, int value)
    {
        Preferences.Set(key, value);
    }

    public void Set(string key, string value)
    {
        Preferences.Set(key, value);
    }
}
