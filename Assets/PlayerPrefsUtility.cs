using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsUtility
{
    public static List<string> GetKeys()
    {
        var keys = new List<string>();
        var type = typeof(PlayerPrefs);
        var info = type.GetField("s_PlayerPrefs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var dict = info.GetValue(null) as Dictionary<string, object>;
        keys.AddRange(dict.Keys);
        return keys;
    }
}
