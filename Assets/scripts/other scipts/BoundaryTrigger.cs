using UnityEngine;
using UnityEngine.Events;

public class BoundaryTrigger : MonoBehaviour
{
    [Tooltip("The tag on your VR Player (e.g., MainCamera).")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("What happens when the player hits this boundary?")]
    public UnityEvent OnHitBoundary;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            OnHitBoundary?.Invoke();
        }
    }
}