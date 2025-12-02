using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LavaTrigger : MonoBehaviour
{
    // Make sure the collider is a trigger when resetting in editor
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the player (checks both the collider's object and its root)
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            // Kill the player instantly → respawns at last checkpoint
            LevelManager.Instance?.PlayerDied();
        }
    }
}
