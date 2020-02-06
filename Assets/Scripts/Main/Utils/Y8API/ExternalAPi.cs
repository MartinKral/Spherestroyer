#if UNITY_WEBGL

using System.Runtime.InteropServices;

public static class ExternalAPI
{
    public delegate void Callback(string callbackType);

    [MonoPInvokeCallback(typeof(Callback))]
    public static void JsCallback(string callbackType)
    {
        Logger.Log($"[Y8]{callbackType}");

        // Dictionary not available
        switch (callbackType)
        {
            case "init":
                Y8.Api.IsInitialized = true;
                break;

            case "login":
                Y8.Api.IsLoggedIn = true;
                break;

            case "score-success":
            case "score-fail":
            default:
                break;
        }
    }

    [DllImport("__Internal")]
    internal static extern void ShowHighscore(string tableId, Callback callback);

    [DllImport("__Internal")]
    internal static extern void Init(string appId, Callback callback);

    [DllImport("__Internal")]
    internal static extern void SaveHighscore(string tableId, int score, Callback callback);
}

#else

public static class ExternalAPI
{
    public delegate void Callback(string callbackType);

    public static void JsCallback(string callbackType)
    {
    }

    internal static void ShowHighscore(string tableId, Callback callback)
    {
    }

    internal static void Init(string appId, Callback callback)
    {
    }

    internal static void SaveHighscore(string tableId, int score, Callback callback)
    {
    }
}

#endif