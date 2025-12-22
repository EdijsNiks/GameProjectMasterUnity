using UnityEngine;
using BNG;

[RequireComponent(typeof(LineRenderer))]
public class GrappleGunVR : MonoBehaviour
{
    [Header("References")]
    public Transform gunTip;
    public Rigidbody playerBody;

    [Header("Grapple Settings")]
    public LayerMask grappleLayer;
    public float maxDistance = 25f;

    [Header("Spring Settings")]
    public float swingSpring = 50f;
    public float swingDamper = 10f;
    public float swingMassScale = 4.5f;

    [Header("Reel-In Settings")]
    public float reelSpeed = 3f;         // m/s reel-in rate
    public float minRopeLength = 1.0f;   // shortest possible rope
    public float pullForce = 10f;       // smooth pull toward anchor

    [Header("Visuals")]
    public Color ropeColor = Color.cyan;
    public float ropeWidth = 0.04f;

        [Header("Aiming Laser")]
    public Color aimColor = Color.red;
    public float aimWidth = 0.005f;

    private LineRenderer rope;
    private SpringJoint joint;
    private InputBridge input;
    private Vector3 hookPoint;
    private float originalDrag;
        private LineRenderer laser;

    void Awake()
    {
        input = InputBridge.Instance;

        if (!gunTip) gunTip = transform;
        if (!playerBody) playerBody = GetComponentInParent<Rigidbody>();

        rope = GetComponent<LineRenderer>();
        rope.positionCount = 0;
        rope.startWidth = rope.endWidth = ropeWidth;
        rope.material = new Material(Shader.Find("Unlit/Color")) { color = ropeColor };

        originalDrag = playerBody.linearDamping;

        // Laser setup
        GameObject laserObj = new GameObject("AimLaser");
        laserObj.transform.SetParent(gunTip);
        laser = laserObj.AddComponent<LineRenderer>();
        laser.positionCount = 2;
        laser.startWidth = laser.endWidth = aimWidth;
        laser.material = new Material(Shader.Find("Unlit/Color"));
        laser.material.color = aimColor;
        laser.enabled = false;
    }

    void Update()
    {
        HandleInput();
        UpdateRope();
        UpdateAimLaser();

    }

    void FixedUpdate()
    {
        if (joint != null)
        {
            HandleReelIn();
        }
    }

    void HandleInput()
    {
        // FIRE
        if (input.RightTriggerDown && joint == null)
            TryHook();

        // RELEASE
        if (input.LeftTriggerDown)
            Detach();
    }

    void TryHook()
    {
        if (Physics.Raycast(gunTip.position, gunTip.forward, out RaycastHit hit, maxDistance, grappleLayer))
        {
            hookPoint = hit.point;
            CreateSpringJoint();
            rope.positionCount = 2;

            input.VibrateController(0.3f, 0.5f, 0.1f, ControllerHand.Right);
            Debug.Log($"[Grapple] Hooked to {hit.collider.name}");
        }
    }

    void CreateSpringJoint()
    {
        joint = playerBody.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = hookPoint;

        float dist = Vector3.Distance(playerBody.position, hookPoint);
        joint.maxDistance = dist * 0.9f;
        joint.minDistance = dist * 0.4f;

        joint.spring = swingSpring;
        joint.damper = swingDamper;
        joint.massScale = swingMassScale;
        joint.enablePreprocessing = false;

        // Slightly increase drag to stabilize swing
        playerBody.linearDamping = 2.5f;
    }

    void HandleReelIn()
    {
        // HOLD right grip to reel in smoothly
        if (input.RightGrip > 0.5f)
        {
            // Shorten rope gradually
            float newMax = joint.maxDistance - reelSpeed * Time.fixedDeltaTime;
            joint.maxDistance = Mathf.Max(newMax, minRopeLength);

            // Apply small directional pull
            Vector3 dir = (hookPoint - playerBody.position).normalized;
            playerBody.AddForce(dir * pullForce, ForceMode.Acceleration);
        }
    }

    void Detach()
    {
        if (joint)
        {
            Destroy(joint);
            joint = null;
        }

        rope.positionCount = 0;

        // Reset physics so player stops sliding or spinning
        playerBody.linearVelocity = Vector3.zero;
        playerBody.angularVelocity = Vector3.zero;
        playerBody.linearDamping = originalDrag;

        // Optional: keep player upright
        Vector3 upright = playerBody.rotation.eulerAngles;
        playerBody.MoveRotation(Quaternion.Euler(0, upright.y, 0));

        Debug.Log("[Grapple] Detached and stabilized");
    }

    void UpdateRope()
    {
        if (joint == null) return;
        rope.SetPosition(0, gunTip.position);
        rope.SetPosition(1, hookPoint);
    }

    void UpdateAimLaser() {
        if (input.RightTrigger > 0.1f && joint == null) {
            laser.enabled = true;

            if (Physics.Raycast(gunTip.position, gunTip.forward, out RaycastHit hit, maxDistance, grappleLayer)) {
                laser.SetPosition(0, gunTip.position);
                laser.SetPosition(1, hit.point);
            }
            else {
                laser.SetPosition(0, gunTip.position);
                laser.SetPosition(1, gunTip.position + gunTip.forward * maxDistance);
            }
        }
        else {
            laser.enabled = false;
        }
    }
}
