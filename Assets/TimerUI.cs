using UnityEngine;
using TMPro;

public class LevelStatsUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text deathsText;

    void Update()
    {
        if (LevelManager.Instance == null) return;

        // Update timer
        if (timerText != null)
        {
            float time = LevelManager.Instance.elapsedTime;
            timerText.text = $"Time: {time:F2}s";
        }

        // Update deaths
        if (deathsText != null)
        {
            int deaths = LevelManager.Instance.deaths;
            deathsText.text = $"Deaths: {deaths}";
        }
    }
}
