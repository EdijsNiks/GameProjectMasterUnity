using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip finishSound;
    [Range(0f, 1f)] public float soundVolume = 1f;

    private AudioSource audioSource;

    [Header("Scene Reload Delay")]
    public float extraDelay = 0.5f; // extra delay after sound

    void Awake()
    {
        if (finishSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D sound
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Play finish sound
        if (audioSource != null && finishSound != null)
            audioSource.PlayOneShot(finishSound, soundVolume);

        // Save PB via LevelManager
        LevelManager.Instance?.SavePB();

        // Start coroutine to reload scene after sound + delay
        StartCoroutine(ReloadSceneAfterDelay());
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        // Wait for sound duration + extra delay
        float waitTime = (finishSound != null ? finishSound.length : 0f) + extraDelay;
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene("MainHub");
    }
}




