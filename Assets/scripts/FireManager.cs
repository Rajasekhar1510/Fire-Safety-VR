using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class FireManager : MonoBehaviour
{
    public static FireManager Instance { get; private set; }

    [Header("Fires to Track")]
    [Tooltip("Drag and drop all your Fire GameObjects here.")]
    [SerializeField] private List<FireReaction> activeFires = new List<FireReaction>();

    [Header("Audio Settings")]
    [Tooltip("Voiceover to play when all fires are extinguished.")]
    [SerializeField] private AudioClip simulationCompleteVoiceover;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent OnSimulationComplete;

    public AudioSource audioSource;
    private bool isComplete = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    /// <summary>
    /// Called by FireReaction.cs when emission reaches 0.
    /// </summary>
    public void ReportFireExtinguished(FireReaction extinguishedFire)
    {
        if (isComplete) return;

        if (activeFires.Contains(extinguishedFire))
        {
            activeFires.Remove(extinguishedFire);

            if (activeFires.Count == 0)
            {
                isComplete = true;
                PlayCompletionAudio();

                // Add this line to trigger the Retry Menu:
                OnSimulationComplete?.Invoke();
            }
        }

    }

    private void PlayCompletionAudio()
    {
        if (simulationCompleteVoiceover != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(simulationCompleteVoiceover);
        }
    }
}