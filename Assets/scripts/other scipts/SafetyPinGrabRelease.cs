using UnityEngine;
using UnityEngine.Events;

public class SafetyPinGrabRelease : MonoBehaviour
{
    [Header("Audio & Events")]
    [Tooltip("Sound of the pin falling/dropping.")]
    [SerializeField] private AudioClip pinDropSound;

    [Tooltip("What happens when the pin is successfully pulled out?")]
    public UnityEvent OnPinPulled;

    private bool hasBeenGrabbed = false;
    private bool isPulled = false;

    /// <summary>
    /// Call this from InteractableUnityEventWrapper -> When Select ()
    /// </summary>
    public void OnPinGrabbed()
    {
        if (isPulled) return;

        hasBeenGrabbed = true;
    }

    /// <summary>
    /// Call this from InteractableUnityEventWrapper -> When Unselect ()
    /// </summary>
    public void OnPinReleased()
    {
        if (isPulled) return;

        // Ensure it drops only if it was actually grabbed, not just hovered
        if (hasBeenGrabbed)
        {
            TriggerPinPulled();
        }
    }

    private void TriggerPinPulled()
    {
        isPulled = true;

        // 1. Play sound detached from this object, so it doesn't cut off
        if (pinDropSound != null)
        {
            AudioSource.PlayClipAtPoint(pinDropSound, transform.position);
        }

        // 2. Notify other scripts (enabling the extinguisher)
        OnPinPulled?.Invoke();

        // 3. Hide and disable the pin
        gameObject.SetActive(false);
    }
}