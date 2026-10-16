using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExtinguisherAudioController : MonoBehaviour
{
    [Header("Voiceover Audio Clips")]
    [Tooltip("Audio explaining the Dry Powder extinguisher.")]
    [SerializeField] private AudioClip dryExtinguisherClip;

    [Tooltip("Audio explaining the Wet Chemical/Foam extinguisher.")]
    [SerializeField] private AudioClip chemicalExtinguisherClip;

    [Tooltip("Audio explaining the Water extinguisher.")]
    [SerializeField] private AudioClip waterExtinguisherClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Changed to 0f for perfectly clear 2D UI audio
        audioSource.spatialBlend = 0f;
    }

    /// <summary>
    /// Link to the Dry Extinguisher UI Button (When Select)
    /// </summary>
    public void PlayDryAudio()
    {
        PlayClip(dryExtinguisherClip);
    }

    /// <summary>
    /// Link to the Chemical Extinguisher UI Button (When Select)
    /// </summary>
    public void PlayChemicalAudio()
    {
        PlayClip(chemicalExtinguisherClip);
    }

    /// <summary>
    /// Link to the Water Extinguisher UI Button (When Select)
    /// </summary>
    public void PlayWaterAudio()
    {
        PlayClip(waterExtinguisherClip);
    }

    // Handles stopping current audio before playing the new one to prevent overlap
    private void PlayClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}