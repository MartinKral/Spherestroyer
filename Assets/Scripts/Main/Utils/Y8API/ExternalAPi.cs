using System;
using System.Runtime.InteropServices;

public static class ExternalAPI
{
    [DllImport("__Internal")]
    internal static extern void ShowHighscore(string tableId);

    [DllImport("__Internal")]
    internal static extern void Init(string appId, Action successCallback);

    [DllImport("__Internal")]
    internal static extern bool IsLoggedIn();

    [DllImport("__Internal")]
    internal static extern void SaveHighscore(string tableId, int score);

#if UNITY_WEBGL
#else

    //public static void ShowHighscore(string tableId)
    //{
    //}

    //public static void Init(string appId)
    //{
    //}

    //public static bool IsLoggedIn()
    //{
    //    return false;
    //}

    //public static void SaveHighscore(string tableId, int score)
    //{
    //}

#endif
}