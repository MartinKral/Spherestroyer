using System;
using System.Runtime.InteropServices;

public static class ExternalAPI
{
#if UNITY_WEBGL
    [MonoPInvokeCallback(typeof(Action))]
    public static void Callback()
    {
        Logger.Log("C# function callback from JS");
    }

    [DllImport("__Internal")]
    public static extern void WithCallback(Action action);

    [DllImport("__Internal")]
    internal static extern void ShowHighscore(string tableId);

    [DllImport("__Internal")]
    internal static extern void Init(string appId);

    [DllImport("__Internal")]
    internal static extern bool IsLoggedIn();

    [DllImport("__Internal")]
    internal static extern void SaveHighscore(string tableId, int score);

#else

    public static void Callback()
    {
    }

    public static void WithCallback(Action action)
    {
    }

    public static void ShowHighscore(string tableId)
    {
    }

    public static void Init(string appId)
    {
    }

    public static bool IsLoggedIn()
    {
        return false;
    }

    public static void SaveHighscore(string tableId, int score)
    {
    }

#endif
}