using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [Tooltip("Transform used for respawning (usually an empty child on the checkpoint).")]
    public Transform respawnPoint;

    [Tooltip("Optional one-time activation (disable after triggered).")]
    public bool singleUse = false;

    [Tooltip("Visual indicator to switch on when checkpoint is active.")]
    public GameObject activeIndicator;

    private bool triggered = false;

    void Reset()
    {
        // ensure collider is trigger by default
        Collider c = GetComponent<Collider>();
        if (c != null) c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // robust check: use root tag "Player"
        if (!IsPlayerCollider(other)) return;
        if (triggered && singleUse) return;

        Transform target = respawnPoint != null ? respawnPoint : transform;
        LevelManager.Instance?.SetCheckpoint(target);

        if (activeIndicator != null) activeIndicator.SetActive(true);

        if (singleUse) triggered = true;
    }

    bool IsPlayerCollider(Collider other)
    {
        return other.transform.root.CompareTag("Player");
    }

}

