using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [Header("Respawn")]
    [Tooltip("Transform used for respawning (usually an empty child on the checkpoint).")]
    public Transform respawnPoint;

    [Header("Checkpoint Options")]
    [Tooltip("Optional one-time activation (disable after triggered).")]
    public bool singleUse = false;
    private bool triggered = false;

    [Header("Visuals")]
    [Tooltip("Visual indicator to switch on when checkpoint is active.")]
    public GameObject activeIndicator;

    [Header("Audio")]
    public AudioClip checkpointSound;
    [Range(0f, 1f)] public float soundVolume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        // Setup audio source automatically
        if (checkpointSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;  // 2D sound, no VR weirdness
        }
    }

    void Reset()
    {
        Collider c = GetComponent<Collider>();
        if (c != null) c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayerCollider(other)) return;
        if (triggered && singleUse) return;

        Transform target = respawnPoint != null ? respawnPoint : transform;
        LevelManager.Instance?.SetCheckpoint(target);

        // visual
        if (activeIndicator != null) activeIndicator.SetActive(true);

        // audio
        PlayCheckpointSound();

        if (singleUse) triggered = true;
    }

    void PlayCheckpointSound()
    {
        if (audioSource != null && checkpointSound != null)
        {
            audioSource.PlayOneShot(checkpointSound, soundVolume);
        }
    }

    bool IsPlayerCollider(Collider other)
    {
        return other.transform.root.CompareTag("Player");
    }
}


