using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PBDisplay : MonoBehaviour
{
    [Header("Settings")]
    public string levelSceneName = "MainHub";
    
    [Header("References")]
    public TMP_Text text;

    void Start()
    {
        if (text == null)
        {
            Debug.LogError("[PBDisplay] TMP_Text reference is missing! Please assign it in the Inspector.");
            return;
        }

        string key = levelSceneName + "_PB";
        string deathsKey = levelSceneName + "_Deaths";

        string displayText = $"<b>{levelSceneName}</b>\n";

        if (PlayerPrefs.HasKey(key))
        {
            float pb = PlayerPrefs.GetFloat(key);
            displayText += $"PB: {pb:F2}s";
        }
        else
        {
            displayText += "PB: --";
        }

        if (PlayerPrefs.HasKey(deathsKey))
        {
            int deaths = PlayerPrefs.GetInt(deathsKey);
            displayText += $"\nDeaths: {deaths}";
        }

        text.text = displayText;
    }
}
