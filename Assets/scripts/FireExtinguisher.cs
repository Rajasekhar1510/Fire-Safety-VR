using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public OVRInput.RawButton shootingButton;
    public ParticleSystem extinguisherParticles;

    [Header("Audio")]
    public AudioSource extinguisherAudio;

    private bool isGrabbed = false;

    // Called by Meta Interaction SDK when grabbed
    public void OnGrabbed()
    {
        isGrabbed = true;
    }

    // Called by Meta Interaction SDK when released
    public void OnReleased()
    {
        isGrabbed = false;

        // Stop particles and sound if dropped while shooting
        if (extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Stop();
        }

        if (extinguisherAudio != null && extinguisherAudio.isPlaying)
        {
            extinguisherAudio.Stop();
        }
    }

    private void Update()
    {
        if (isGrabbed)
        {
            if (OVRInput.GetDown(shootingButton))
            {
                extinguisherParticles.Play();

                // Play the sound if it isn't already playing
                if (extinguisherAudio != null && !extinguisherAudio.isPlaying)
                {
                    extinguisherAudio.Play();
                }

                Debug.Log("Shooting button pressed, playing particles and sound.");
            }
            else if (OVRInput.GetUp(shootingButton))
            {
                extinguisherParticles.Stop();

                // Stop the sound
                if (extinguisherAudio != null && extinguisherAudio.isPlaying)
                {
                    extinguisherAudio.Stop();
                }

                Debug.Log("Shooting button released, stopping particles and sound.");
            }
        }
    }
}