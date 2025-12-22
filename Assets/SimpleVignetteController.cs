using UnityEngine;

public class SimpleVignetteController : MonoBehaviour
{
    public Transform rig;                 // Your XR Rig root
    public GameObject vignetteMesh;       // The XR-IT vignette mesh prefab
    public float speedThreshold = 0.1f;   // When vignette activates

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = rig.position;
        vignetteMesh.SetActive(false); // start disabled
    }

    void Update()
    {
        float speed = (rig.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = rig.position;

        if (speed > speedThreshold)
        {
            vignetteMesh.SetActive(true);
        }
        else
        {
            vignetteMesh.SetActive(false);
        }
    }
}
