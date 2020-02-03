#if UNITY_WEBGL

using System.Runtime.InteropServices;

public static class ExternalAPI
{
    public delegate void Callback(string callbackType);

    [MonoPInvokeCallback(typeof(Callback))]
    public static void JsCallback(string callbackType)
    {
        Logger.Log($"[Y8]{callbackType}");

        if (callbackType == "init")
        {
            Y8.Api.IsInitialized = true;
        }

        if (callbackType == "login")
        {
            Y8.Api.IsInitialized = true;
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

    public static void ShowHighscore(string tableId, Callback callback)
    {
    }

    public static void Init(string appId, Callback callback)
    {
    }

    public static void SaveHighscore(string tableId, int score, Callback callback)
    {
    }
}

#endif