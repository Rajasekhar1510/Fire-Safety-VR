using UnityEngine;

public class GoggleEquipTrigger : MonoBehaviour
{
    [Tooltip("Drag your GoggleManager here.")]
    [SerializeField] private GoggleManager goggleManager;

    [Tooltip("The tag on your VR Player's head/camera.")]
    [SerializeField] private string headTag = "MainCamera";

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we bumped into is the player's head
        if (other.CompareTag(headTag))
        {
            if (goggleManager != null)
            {
                // This will remove the boundary, play the sound, and hide the goggles
                goggleManager.OnGogglesEquipped();
            }
        }
    }
}