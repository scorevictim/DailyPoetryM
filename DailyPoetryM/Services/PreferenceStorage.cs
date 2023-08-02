﻿namespace DailyPoetryM.Services;

public class PreferenceStorage : IPreferenceStorage
{
    public int Get(string key, int defaultValue)
    {
        return Preferences.Get(key, defaultValue);
    }

    public void Set(string key, int value)
    {
        Preferences.Set(key, value);
    }
}
