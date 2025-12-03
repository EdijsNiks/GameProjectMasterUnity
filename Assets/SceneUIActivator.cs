using UnityEngine;

public class SceneUIActivator : MonoBehaviour
{
    [Header("UI object to toggle")]
    public GameObject levelUI;

    [Header("Teleport point that triggers UI")]
    public Transform teleportPoint; // Assign the specific empty GameObject

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (levelUI != null)
            levelUI.SetActive(false);
    }

    void Update()
    {
        if (levelUI == null || player == null || teleportPoint == null) return;

        // Show UI only when player is at the teleport point
        float distance = Vector3.Distance(player.position, teleportPoint.position);
        bool shouldBeVisible = distance < 0.5f; // Adjust tolerance if needed
        levelUI.SetActive(shouldBeVisible);
    }
}

