using UnityEngine;

public class Sanity
{
    private float maxSanity;
    public float currentSanity;
    private float drainSanityPerSecond;

    public Sanity(float max, float drain) {
        maxSanity = max;
        drainSanityPerSecond = drain;

        currentSanity = maxSanity;
    }

    public void LoseSanity(float amount) {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

        Debug.Log("Sanity: " + currentSanity);
    }

    public void GainSanity(float amount) {
        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

        Debug.Log("Sanity: " + currentSanity);
    }

    public void DrainSanity(float amount) {
        currentSanity -= drainSanityPerSecond * Time.deltaTime;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
    }
    
}
