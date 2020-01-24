#if UNITY_WEBGL

using System.Runtime.InteropServices;

public class LocalStorage
{
    [DllImport("__Internal")]
    public static extern string GetLocalStorageItem(string key);

    [DllImport("__Internal")]
    public static extern void SetLocalStorageItem(string key, string value);
}
#elif UNITY_EDITOR

using UnityEngine;

public class LocalStorage
{
    public static string GetLocalStorageItem(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static void SetLocalStorageItem(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
}

#else
public class LocalStorage
{
    public static string GetLocalStorageItem(string key)
    {
        return "0";
    }

    public static void SetLocalStorageItem(string key, string value)
    {
    }
}

#endif