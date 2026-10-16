using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OneTimeAudioTrigger : MonoBehaviour
{
    [Tooltip("The tag on your VR Player's head/camera.")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("The voiceover to play when the player enters.")]
    [SerializeField] private AudioClip extinguisherVoiceover;

    private AudioSource audioSource;
    private bool hasPlayed = false;

    private void Awake()
    {
        // Grab the AudioSource on this object
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If it's the player, and we haven't played the audio yet
        if (!hasPlayed && other.CompareTag(playerTag))
        {
            hasPlayed = true; // Make sure it never plays again

            if (extinguisherVoiceover != null)
            {
                audioSource.PlayOneShot(extinguisherVoiceover);
            }
        }
    }
}