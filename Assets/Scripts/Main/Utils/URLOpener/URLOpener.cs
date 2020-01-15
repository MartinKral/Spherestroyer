#if UNITY_EDITOR

using UnityEngine;

public class URLOpener
{
    public static void OpenURL(string url)
    {
        Debug.Log($"Clicked to open URL: {url}");
    }
}

#elif UNITY_WEBGL

using System.Runtime.InteropServices;

public class URLOpener
{
    [DllImport("__Internal")]
    public static extern void OpenURL(string url);
}

#else

public class URLOpener
{
    public static void OpenURL(string url)
    {
    }
}

#endif