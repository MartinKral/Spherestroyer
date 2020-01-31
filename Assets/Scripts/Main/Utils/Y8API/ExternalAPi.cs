using System;
using System.Runtime.InteropServices;

public static class ExternalAPI
{
    [MonoPInvokeCallback(typeof(Action))]
    public static void Callback()
    {
        Logger.Log("C# function callback from JS");
    }

    [DllImport("__Internal")]
    public static extern void ProvideCallback(Action action);

    [DllImport("__Internal")]
    internal static extern void ShowHighscore(string tableId);

    [DllImport("__Internal")]
    internal static extern void Init(string appId);

    [DllImport("__Internal")]
    internal static extern bool IsLoggedIn();

    [DllImport("__Internal")]
    internal static extern void SaveHighscore(string tableId, int score);
}