using UnityEngine;
using Unity.XR.CoreUtils;

public class LevelStart : MonoBehaviour
{
    public Transform spawnPoint;

    private void Start()
    {
        XROrigin rig = FindObjectOfType<XROrigin>();

        // Move rig to spawn
        rig.transform.SetPositionAndRotation(
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}
