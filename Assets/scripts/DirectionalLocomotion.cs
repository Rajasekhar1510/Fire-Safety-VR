using UnityEngine;

[DefaultExecutionOrder(1000)] // Ensures this runs after the OVRCameraRig updates
public class DirectionalLocomotion : MonoBehaviour
{
    [Header("Rig References")]
    [Tooltip("The root of your OVRCameraRig.")]
    public Transform rigRoot;
    [Tooltip("The CenterEyeAnchor inside the TrackingSpace of OVRCameraRig.")]
    public Transform headTransform;

    [Header("Movement Settings")]
    [Tooltip("How much virtual distance is covered per physical step forward. 1 = normal speed, 3 = 3x speed.")]
    [Min(1f)] public float forwardMovementScale = 3f;
    [Tooltip("Ignore tiny tracking jitters smaller than this value.")]
    public float deadzone = 0.001f;

    private Vector3 lastHeadLocal;
    private bool isInitialized = false;

    void Start()
    {
        if (rigRoot != null && headTransform != null)
        {
            lastHeadLocal = headTransform.localPosition;
            isInitialized = true;
        }
        else
        {
            Debug.LogError("DirectionalLocomotion: Please assign rigRoot and headTransform in the inspector.");
        }
    }

    void LateUpdate()
    {
        if (!isInitialized) return;

        // 1. Calculate how much the headset moved physically this frame
        Vector3 localNow = headTransform.localPosition;
        Vector3 localDelta = localNow - lastHeadLocal;
        lastHeadLocal = localNow;

        Transform parent = headTransform.parent;
        Vector3 worldDelta = parent != null ? parent.TransformDirection(localDelta) : localDelta;
        worldDelta.y = 0f; // Keep calculations strictly on the horizontal plane

        if (worldDelta.magnitude < deadzone) return;

        // 2. Determine the player's current physical facing direction
        Vector3 headForward = headTransform.forward;
        headForward.y = 0f;
        if (headForward.sqrMagnitude < deadzone) return;
        headForward.Normalize();

        // 3. Project the physical step onto the player's forward direction
        float forwardStepAmount = Vector3.Dot(worldDelta, headForward);

        if (forwardStepAmount > 0)
        {
            // WALKING FORWARD: Multiply the movement
            // The OVRCameraRig naturally moves the player 1x. We add the extra multiplier here.
            float extraMovement = forwardStepAmount * (forwardMovementScale - 1f);
            rigRoot.position += headForward * extraMovement;
        }
        else if (forwardStepAmount < 0)
        {
            // WALKING BACKWARD: Cancel the movement
            // Because the physical headset moved back, the camera goes back in VR.
            // We counter this by pushing the rigRoot forward by the exact same distance.
            rigRoot.position += headForward * Mathf.Abs(forwardStepAmount);
        }

        // Note: Side-stepping is left 1:1 natively. If you want to cancel sideways movement too, 
        // you would subtract the sidestep projection from the rigRoot as well.
    }
}