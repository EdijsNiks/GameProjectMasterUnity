using UnityEngine;

public class SnowTrigger : MonoBehaviour
{
    [Header("Snow Particle System")]
    public ParticleSystem snowParticles;

    [Header("Settings")]
    public bool oneTimeActivation = true;
    private bool hasActivated = false;

    void Start()
    {
        // Make sure snow starts disabled
        if (snowParticles != null)
        {
            snowParticles.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if already activated
        if (oneTimeActivation && hasActivated)
            return;

        // Simple player check - just check the tag
        if (other.CompareTag("Player"))
        {
            ActivateSnow();
        }
    }

    void ActivateSnow()
    {
        if (snowParticles != null)
        {
            snowParticles.Play();
            hasActivated = true;
            Debug.Log("[SnowTrigger] Snow activated!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !oneTimeActivation)
        {
            if (snowParticles != null)
            {
                snowParticles.Stop();
            }
        }
    }
}