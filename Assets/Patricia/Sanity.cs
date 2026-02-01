using UnityEngine;

public class Sanity
{
    private float maxSanity;
    public float currentSanity;
    private float drainSanityPerSecond;

    private HudManager HM;

    public Sanity(float max, float drain) {
        maxSanity = max;
        drainSanityPerSecond = drain;

        currentSanity = maxSanity;

        GameObject obj = GameObject.Find("Hud");
        HM = obj.GetComponent<HudManager>();
    }

    public void LoseSanity(float amount) {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

        Debug.Log("Sanity: " + currentSanity);
        HM.SetSanityBar(currentSanity / maxSanity);
    }

    public void GainSanity(float amount) {
        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

        Debug.Log("Sanity: " + currentSanity);
        HM.SetSanityBar(currentSanity / maxSanity);
    }

    public void DrainSanity(float amount) {
        currentSanity -= drainSanityPerSecond * Time.deltaTime;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

        HM.SetSanityBar(currentSanity / maxSanity);
    }
    
}
