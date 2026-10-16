using UnityEngine;
using UnityEngine.Events;

public class SafetyPinDistanceCheck : MonoBehaviour
{
    [Header("Distance Settings")]
    [Tooltip("The local Z position the pin must reach to be considered pulled.")]
    [SerializeField] private float targetZPosition = -0.25f;

    [Tooltip("Check this if the pin is moving into negative numbers (e.g. from 0 to -0.25). Uncheck if moving into positive numbers.")]
    [SerializeField] private bool movingIntoNegative = true;

    [Header("Audio & Events")]
    [Tooltip("Sound of the pin falling/dropping.")]
    [SerializeField] private AudioClip pinDropSound;

    [Tooltip("What happens when the pin is successfully pulled out?")]
    public UnityEvent OnPinPulled;

    private bool isPulled = false;

    private void Update()
    {
        // Stop checking if it's already been pulled
        if (isPulled) return;

        // Check if the pin has reached or passed the target Z position
        bool targetReached = movingIntoNegative
            ? transform.localPosition.z <= targetZPosition
            : transform.localPosition.z >= targetZPosition;

        if (targetReached)
        {
            isPulled = true;
            TriggerPinPulled();
        }
    }

    private void TriggerPinPulled()
    {
        // 1. Play sound detached from this object, so it doesn't cut off
        if (pinDropSound != null)
        {
            AudioSource.PlayClipAtPoint(pinDropSound, transform.position);
        }

        // 2. Notify other scripts (e.g., turning on the extinguisher script)
        OnPinPulled?.Invoke();

        // 3. Hide and disable the pin
        gameObject.SetActive(false);
    }
}