using UnityEngine;

public static class Log
{
    public static void Warning(string msg)
    {
#if UNITY_EDITOR
        Debug.LogWarning(msg);
#endif
    }

    public static void Info(string msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }

    public static void Error(string msg)
    {
#if UNITY_EDITOR
        Debug.LogError(msg);
#endif
    }
}
