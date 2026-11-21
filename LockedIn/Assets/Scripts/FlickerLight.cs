using UnityEngine;

// Makes a light flicker randomly (like a dying lamp) until SetSteady() is called.
// Use it on the desk lamp before power is restored. Call SetSteady() from the fuse box.
[RequireComponent(typeof(Light))]
public class FlickerLight : MonoBehaviour
{
    [Header("Flicker")]
    [SerializeField] private bool flickerOnStart = true;
    [SerializeField] private float minIntensity = 0.2f;
    [SerializeField] private float maxIntensity = 1.2f;
    [SerializeField] private float minInterval = 0.03f;
    [SerializeField] private float maxInterval = 0.25f;

    [Header("Steady")]
    [SerializeField] private float steadyIntensity = 2f;

    [Header("Audio (optional)")]
    [SerializeField] private AudioSource buzzSound;

    private Light lightSource;
    private bool flickering;
    private float nextChange;

    private void Awake()
    {
        lightSource = GetComponent<Light>();
        flickering = flickerOnStart;
    }

    private void Update()
    {
        if (!flickering || Time.time < nextChange) return;

        lightSource.intensity = Random.Range(minIntensity, maxIntensity);
        nextChange = Time.time + Random.Range(minInterval, maxInterval);
    }

    // Stop flickering and stay on at full brightness.
    public void SetSteady()
    {
        flickering = false;
        lightSource.enabled = true;
        lightSource.intensity = steadyIntensity;
        if (buzzSound != null) buzzSound.Stop();
    }

    public void StartFlicker()
    {
        flickering = true;
    }
}
