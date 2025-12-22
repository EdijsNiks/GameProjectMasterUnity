using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [Header("UI Timer to Show")]
    public GameObject timerUI; // Assign your timer GameObject

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Start the level via LevelManager
        LevelManager.Instance.StartLevel();

        // Show the timer UI
        if (timerUI != null)
            timerUI.SetActive(true);

        Destroy(gameObject);
    }
}

