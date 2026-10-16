using UnityEngine;

public class GoggleManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The physical Goggles GameObject the player will grab.")]
    [SerializeField] private GameObject gogglesObject;

    [Tooltip("The Quad boundary that blocks the player until goggles are worn.")]
    [SerializeField] private GameObject goggleBoundaryQuad;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip putOnGogglesVoiceover; // E.g., "Please wear your safety goggles."
    [SerializeField] private AudioClip equipSound; // A physical snap/click sound

    private void Awake()
    {
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        // Hide goggles and activate the blocking boundary on start
        if (gogglesObject != null) gogglesObject.SetActive(false);
        if (goggleBoundaryQuad != null) goggleBoundaryQuad.SetActive(true);
    }

    /// <summary>
    /// Call this from StartUIController's OnStartClickedEvent
    /// </summary>
    public void SpawnGoggles()
    {
        if (gogglesObject != null)
        {
            gogglesObject.SetActive(true);
        }
    }

    /// <summary>
    /// Call this from the Goggle Boundary's BoundaryTrigger script
    /// </summary>
    public void PlayBoundaryWarning()
    {
        if (putOnGogglesVoiceover != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(putOnGogglesVoiceover);
        }
    }

    /// <summary>
    /// Call this from the Meta SnapZone's InteractableUnityEventWrapper (When Select)
    /// </summary>
    public void OnGogglesEquipped()
    {
        // 1. Remove the barrier
        if (goggleBoundaryQuad != null) goggleBoundaryQuad.SetActive(false);

        // 2. Play the equip sound
        if (equipSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(equipSound);
        }

        // 3. Hide the physical goggles so they don't block the camera view
        // (Optional: You could enable a screen-tint or post-processing volume here instead)
        if (gogglesObject != null)
        {
            gogglesObject.SetActive(false);
        }
    }
}