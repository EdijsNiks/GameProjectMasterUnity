using UnityEngine;

public class PlayerPrefDebugger : MonoBehaviour
{
    void Start()
    {
        foreach (var key in PlayerPrefsUtility.GetKeys())
        {
            Debug.Log("PlayerPref Key: " + key + " = " + PlayerPrefs.GetFloat(key));
        }
    }
}
