using UnityEngine;
using UnityEngine.Events;

public class SanityVignetteController : MonoBehaviour
{
    [Header("Material Reference")]
    [SerializeField] private Material vignetteMaterial;

    [Header("Sanity Mapping")]
    [Tooltip("At this sanity level, effect starts appearing")]
    [SerializeField] private float sanityThreshold = 0.7f;

    [Tooltip("At this sanity level, effect is at maximum")]
    [SerializeField] private float sanityCritical = 0.2f;

    [Header("Effect Ranges")]
    [SerializeField] private Vector2 vignetteIntensityRange = new Vector2(0f, 0.8f);
    [SerializeField] private Vector2 rayIntensityRange = new Vector2(0f, 0.6f);
    [SerializeField] private Vector2 rayLengthRange = new Vector2(0.05f, 0.3f);
    [SerializeField] private Vector2 waveAmplitudeRange = new Vector2(0f, 0.05f);
    [SerializeField] private Vector2 pulseSpeedRange = new Vector2(0.5f, 3f);

    [Header("Smoothing")]
    [SerializeField] private float transitionSpeed = 2f;

    [Header("Debug")]
    [SerializeField] private bool debugMode;
    [Range(0, 1)]
    [SerializeField] private float debugSanity = 1f;

    // Shader property IDs (cached)
    private static readonly int VignetteIntensityID = Shader.PropertyToID("_VignetteIntensity");
    private static readonly int RayIntensityID = Shader.PropertyToID("_RayIntensity");
    private static readonly int RayLengthID = Shader.PropertyToID("_RayLength");
    private static readonly int WaveAmplitudeID = Shader.PropertyToID("_WaveAmplitude");
    private static readonly int PulseSpeedID = Shader.PropertyToID("_PulseSpeed");

    private float currentSanity = 1f;
    private float targetSanity = 1f;
    private float currentEffectStrength;

    public float CurrentEffectStrength => currentEffectStrength;

    private void Update()
    {
        if (vignetteMaterial == null) return;

        // Use debug value if in debug mode
        float sanity = debugMode ? debugSanity : targetSanity;

        // Smooth transition
        currentSanity = Mathf.MoveTowards(currentSanity, sanity, transitionSpeed * Time.deltaTime);

        // Calculate effect strength (0 = no effect, 1 = full effect)
        currentEffectStrength = CalculateEffectStrength(currentSanity);

        // Apply to material
        ApplyEffect(currentEffectStrength);
    }

    private float CalculateEffectStrength(float sanity)
    {
        // No effect above threshold
        if (sanity >= sanityThreshold) return 0f;

        // Full effect at or below critical
        if (sanity <= sanityCritical) return 1f;

        // Lerp between threshold and critical
        return 1f - Mathf.InverseLerp(sanityCritical, sanityThreshold, sanity);
    }

    private void ApplyEffect(float strength)
    {
        vignetteMaterial.SetFloat(VignetteIntensityID, Mathf.Lerp(vignetteIntensityRange.x, vignetteIntensityRange.y, strength));
        vignetteMaterial.SetFloat(RayIntensityID, Mathf.Lerp(rayIntensityRange.x, rayIntensityRange.y, strength));
        vignetteMaterial.SetFloat(RayLengthID, Mathf.Lerp(rayLengthRange.x, rayLengthRange.y, strength));
        vignetteMaterial.SetFloat(WaveAmplitudeID, Mathf.Lerp(waveAmplitudeRange.x, waveAmplitudeRange.y, strength));
        vignetteMaterial.SetFloat(PulseSpeedID, Mathf.Lerp(pulseSpeedRange.x, pulseSpeedRange.y, strength));
    }


    // Call this from  Sanity system when sanity changes.
    public void SetSanity(float normalizedSanity)
    {
        targetSanity = Mathf.Clamp01(normalizedSanity);
    }

    public void SetSanityImmediate(float normalizedSanity)
    {
        targetSanity = Mathf.Clamp01(normalizedSanity);
        currentSanity = targetSanity;
    }

    // Reset effect to full sanity.
    public void ResetEffect()
    {
        SetSanity(1f);
    }

    private void OnValidate()
    {
        // critical is below threshold
        if (sanityCritical >= sanityThreshold)
        {
            sanityCritical = sanityThreshold - 0.1f;
        }
    }
}