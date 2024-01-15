using System;
using UnityEngine;

public static class Log
{
#if UNITY_EDITOR
    public static void Warning(string msg)
    {
        Debug.LogWarning(msg);
    }

    public static void Info(string msg)
    {
        Debug.Log(msg);
    }

    public static void Error(string msg)
    {
        Debug.LogError(msg);
    }
#endif
}
