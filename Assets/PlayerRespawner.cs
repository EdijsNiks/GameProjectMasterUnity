using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody playerRigidbody;

    void Awake()
    {
        // Auto-find Rigidbody if not assigned
        if (playerRigidbody == null)
            playerRigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Teleports this GameObject to the specified position and rotation.
    /// Resets Rigidbody velocity to prevent momentum carry-over.
    /// Called from LevelManager.RespawnPlayer()
    /// </summary>
    public void RespawnTo(Vector3 position, Quaternion rotation)
    {
        // Reset velocity first to prevent physics from moving player after teleport
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        // Teleport
        transform.position = position;
        transform.rotation = rotation;
    }
}