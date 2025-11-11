using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FinishLine : MonoBehaviour
{
    public AudioClip winSound;        // Assign a sound in the Inspector
    public Text winMessageText;       // Assign a UI Text element (optional)
    public float restartDelay = 2f;   // Delay before restarting

    private AudioSource audioSource;

    void Start()
    {
        // Try to find or add an AudioSource on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Hide the win message at start
        if (winMessageText != null)
        {
            winMessageText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the finish line!");

            // Play sound
            if (winSound != null)
            {
                audioSource.PlayOneShot(winSound);
            }

            // Show message
            if (winMessageText != null)
            {
                winMessageText.gameObject.SetActive(true);
                winMessageText.text = "You Win!";
            }

            // Restart scene after delay
            StartCoroutine(RestartAfterDelay(restartDelay));
        }
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
