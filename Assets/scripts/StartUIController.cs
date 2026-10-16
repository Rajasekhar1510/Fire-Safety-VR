using System;
using UnityEngine;
using UnityEngine.Events;

public class StartUIController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The root UI GameObject/Canvas to show at start and hide upon clicking.")]
    [SerializeField] private GameObject startUIPanel;

    [Header("Boundary Settings")]
    [Tooltip("The Quad barrier that blocks the player.")]
    [SerializeField] private GameObject startBoundaryQuad;
    [Tooltip("Audio played when the player hits the boundary before starting.")]
    [SerializeField] private AudioClip warningVoiceoverClip;

    [Header("Audio Settings")]
    [Tooltip("Audio played once on entering playmode instructing the user to click start.")]
    [SerializeField] private AudioSource voiceoverSource;
    [SerializeField] private AudioClip instructionVoiceoverClip;

    [Tooltip("Audio played when the user clicks the start button.")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip buttonClickClip;

    [Header("Observer Pattern Events")]
    public UnityEvent OnStartClickedEvent;
    public static event Action OnSimulationStarted;

    private void Awake()
    {
        if (voiceoverSource == null) voiceoverSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

        voiceoverSource.spatialBlend = 0f;
        sfxSource.spatialBlend = 0f;
    }

    private void Start()
    {
        if (startUIPanel != null) startUIPanel.SetActive(true);
        if (startBoundaryQuad != null) startBoundaryQuad.SetActive(true);

        PlayInstructionVoiceover();
    }

    private void PlayInstructionVoiceover()
    {
        if (instructionVoiceoverClip != null && voiceoverSource != null)
        {
            voiceoverSource.clip = instructionVoiceoverClip;
            voiceoverSource.Play();
        }
    }

    /// <summary>
    /// Called by the BoundaryTrigger script when the player hits the Quad.
    /// </summary>
    public void PlayWarningVoiceover()
    {
        if (warningVoiceoverClip != null && voiceoverSource != null)
        {
            // Only play if audio isn't currently playing to prevent sound-spam
            // when the player rubs continuously against the quad.
            if (!voiceoverSource.isPlaying)
            {
                voiceoverSource.PlayOneShot(warningVoiceoverClip);
            }
        }
    }

    public void OnStartButtonClicked()
    {
        if (voiceoverSource != null && voiceoverSource.isPlaying) voiceoverSource.Stop();
        if (buttonClickClip != null && sfxSource != null) sfxSource.PlayOneShot(buttonClickClip);

        // Hide UI and the Quad Barrier
        if (startUIPanel != null) startUIPanel.SetActive(false);
        if (startBoundaryQuad != null) startBoundaryQuad.SetActive(false);

        OnStartClickedEvent?.Invoke();
        OnSimulationStarted?.Invoke();
    }
}