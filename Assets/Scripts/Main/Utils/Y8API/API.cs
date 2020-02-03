using System;

public class API
{
    public static bool IsInitialized { get; private set; }
    public bool IsLoggedIn { get; private set; }

    ///public delegate void CallbackDelegate(string )

    public API(string appId)
    {
        ExternalAPI.Init(appId, SuccessCallback);
    }

    [MonoPInvokeCallback(typeof(Action))]
    public static void SuccessCallback()
    {
        Logger.Log($"Y8 initialized");
        IsInitialized = true;
    }

    public void ShowHighscore(string tableId)
    {
        if (!IsInitialized) return;
        ExternalAPI.ShowHighscore(tableId);
    }

    public void SaveHighscore(string tableId, int score)
    {
        if (!IsInitialized) return;
        ExternalAPI.SaveHighscore(tableId, score);
    }
}