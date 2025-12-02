using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporter : MonoBehaviour
{
    [Tooltip("Write the name of the scene you want to load.")]
    public string targetScene;

    private void OnTriggerEnter(Collider other)
    {
        // Only load if Player enters
        if (!other.transform.root.CompareTag("Player")) return;

        // Make sure a scene name was assigned
        if (string.IsNullOrEmpty(targetScene))
        {
            Debug.LogWarning("SceneTeleporter is missing a scene name!");
            return;
        }

        SceneManager.LoadScene(targetScene);
    }
}
