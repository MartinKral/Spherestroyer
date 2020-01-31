#if UNITY_DOTSPLAYER
using Unity.Tiny;

public static class Logger
{
    public static void Log(object message)
    {
        Debug.LogAlways(message);
    }

    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    public static void LogError(object message)
    {
        Debug.LogError(message);
    }
}
#else

using UnityEngine;

public static class Logger
{
    public static void Log(object message)
    {
        Debug.Log(message);
    }

    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    public static void LogError(object message)
    {
        Debug.LogError(message);
    }
}

#endif