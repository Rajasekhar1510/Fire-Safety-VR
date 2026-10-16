using UnityEngine;
using UnityEngine.UI; // Required for interacting with UI elements

public class FireReaction : MonoBehaviour
{
    public ParticleSystem fireParticles;

    [Header("UI & Audio")]
    [Tooltip("Drag the Slider component representing the fire's health here")]
    public Slider healthBarSlider;
    public AudioSource fireAudio;

    [Header("Collider-less Setup")]
    public ParticleSystem extinguisherSystem;
    public Transform fireHitCenter;
    public float hitRadius = 0.5f;
    public float extinguishRate = 15f;

    private ParticleSystem.EmissionModule fireEmission;
    private ParticleSystem.Particle[] particles;
    private float maxEmission; // Stores the starting emission rate

    void Start()
    {
        if (fireParticles != null)
        {
            fireEmission = fireParticles.emission;
            maxEmission = fireEmission.rateOverTime.constant; // Capture the starting emission
        }

        // Configure the health bar slider
        if (healthBarSlider != null)
        {
            healthBarSlider.minValue = 0f;
            healthBarSlider.maxValue = 1f;
            healthBarSlider.value = 1f; // Starts at 100% (1.0)
        }

        if (extinguisherSystem != null)
        {
            particles = new ParticleSystem.Particle[extinguisherSystem.main.maxParticles];
        }

        if (fireHitCenter == null)
        {
            fireHitCenter = transform;
        }

        if (fireAudio != null)
        {
            fireAudio.loop = true;
            if (!fireAudio.isPlaying) fireAudio.Play();
        }
    }

    void Update()
    {
        if (extinguisherSystem == null || !extinguisherSystem.isPlaying) return;

        int numParticlesAlive = extinguisherSystem.GetParticles(particles);
        bool isBeingHit = false;

        bool isLocalSpace = extinguisherSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local;

        for (int i = 0; i < numParticlesAlive; i++)
        {
            Vector3 particlePos = isLocalSpace
                ? extinguisherSystem.transform.TransformPoint(particles[i].position)
                : particles[i].position;

            if (Vector3.Distance(particlePos, fireHitCenter.position) <= hitRadius)
            {
                isBeingHit = true;
                break;
            }
        }

        if (isBeingHit)
        {
            ExtinguishFire();
        }
    }

    private void ExtinguishFire()
    {
        float currentEmission = fireEmission.rateOverTime.constant;

        if (currentEmission > 0)
        {
            currentEmission -= extinguishRate * Time.deltaTime;
            currentEmission = Mathf.Max(0, currentEmission);

            fireEmission.rateOverTime = currentEmission;

            // Calculate the percentage and update the slider (0 to 1)
            if (healthBarSlider != null && maxEmission > 0)
            {
                healthBarSlider.value = currentEmission / maxEmission;
            }

            if (currentEmission <= 0)
            {
                if (fireParticles.isPlaying) fireParticles.Stop();
                if (fireAudio != null && fireAudio.isPlaying) fireAudio.Stop();

                // Optional: Hide the health bar when the fire is completely out
                if (healthBarSlider != null)
                {
                    healthBarSlider.gameObject.SetActive(false);
                }
            }
        }
    }
}