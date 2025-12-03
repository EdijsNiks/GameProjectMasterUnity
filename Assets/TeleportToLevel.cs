using UnityEngine;

public class SceneTeleporter : MonoBehaviour
{
    [Tooltip("Assign the empty GameObject where the player should be teleported.")]
    public Transform playerSpawn;

    [Tooltip("Assign the specific object to teleport (e.g., PlayerController)")]
    public Transform objectToTeleport;

    private void OnTriggerEnter(Collider other)
    {
        // Only teleport if the Player enters
        if (!other.CompareTag("Player")) return;

        // Make sure spawn point and object are assigned
        if (playerSpawn == null)
        {
            Debug.LogWarning("SceneTeleporter is missing a player spawn point!");
            return;
        }

        if (objectToTeleport == null)
        {
            Debug.LogWarning("SceneTeleporter is missing the object to teleport!");
            return;
        }

        // Teleport the assigned object
        objectToTeleport.position = playerSpawn.position;
        objectToTeleport.rotation = playerSpawn.rotation;

        // Reset physics if it has a Rigidbody
        Rigidbody rb = objectToTeleport.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
