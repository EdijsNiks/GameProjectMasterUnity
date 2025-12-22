using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuVR : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load when 'Start' is pressed")]
    public string gameSceneName = "GameScene";

    public void StartGame()
    {
        Debug.Log("Start Game pressed — loading scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game pressed — exiting application");
        Application.Quit();

    }

    public void OpenSettings()
    {
        Debug.Log("Settings button pressed — would open settings menu.");
        // Here you can enable another Canvas or settings panel if you have one
    }

    public void BackToMainMenu()
    {
        Debug.Log("Back button pressed — returning to main menu.");
        SceneManager.LoadScene("MainMenu");
    }
}

