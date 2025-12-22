using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Called when the button is pressed
    public void RestartScene()
    {
        Debug.Log("Restart button pressed — reloading current scene.");
        Time.timeScale = 1f; // Ensure game isn’t paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
