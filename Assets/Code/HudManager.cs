using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HudManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> SanityBarLevels;
    [SerializeField]
    List<GameObject> Feathers;
    [SerializeField]
    GameObject RavenMask;
    [SerializeField]
    GameObject SwanMask;
    [SerializeField]
    GameObject HeldItemObj;
    Image HeldItem;


    [SerializeField]
    private float BlinkDuration = 1.5f;
    [SerializeField]
    private float BlinkInterval = 0.33f;
    private float CurrentPercentage;
    private bool Blink;
    private bool HealBlink;
    private float BlinkTimer;
    private float BlinkDamageThreshold = 0.04f;

    private float LastSanityPercentage = 1.0f;

    private int ActiveSanity = 0;

    public void Start() 
    {
        Blink = false;
        CurrentPercentage = 1.0f;
        HeldItem = HeldItemObj.GetComponent<Image>();
    }

    public void Update() 
    {
        BlinkTimer -= Time.deltaTime;
        if (Blink && BlinkTimer < 0)
        {
            Blink = false;
            SetSanityColor(Color.white);

            foreach (GameObject obj in SanityBarLevels)
            {
                obj.SetActive(false);
            }

            SanityBarLevels[ActiveSanity].SetActive(true);
        }

        if (Blink)
        {
            if (HealBlink)
            {
                SetSanityColor(Color.green);
            }
            else
            {
                SetSanityColor(Color.softRed);
            }

            bool visible = (int)(BlinkTimer / BlinkInterval) % 2 == 1;

            
            foreach (GameObject obj in SanityBarLevels)
            {
                obj.SetActive(false);
            }
            SanityBarLevels[ActiveSanity].SetActive(visible);
           
        }
    }
    
    public void SetSanityBar(float percentage)
    {
        float delta = LastSanityPercentage - percentage;
        if (Mathf.Abs(delta) >= BlinkDamageThreshold)
        {
            Blink = true;
            if (delta > 0)
                HealBlink = false;
            else
                HealBlink = true;
            BlinkTimer = BlinkDuration;
        }
       
        
        ActiveSanity = (SanityBarLevels.Count - 1) - (int)Mathf.Round((SanityBarLevels.Count - 1) * (1 - percentage));


        if (GetCurrentSanity() != ActiveSanity)
        {
            foreach (GameObject obj in SanityBarLevels)
            {
                obj.SetActive(false);
            }

            SanityBarLevels[ActiveSanity].SetActive(true);
        }

        LastSanityPercentage = percentage;
    }

    private int GetCurrentSanity()
    {
        for(int i = 0; i < SanityBarLevels.Count; i++)
        {
            if (SanityBarLevels[i].active)
            {
                return i;
            }
        }
        return -1;
    }

    public void SetMask(bool toRaven)
    {
        RavenMask.SetActive(toRaven);
        SwanMask.SetActive(!toRaven);
    }

    public void SetHeldItem(SpriteRenderer takeSpriteFrom)
    {
        HeldItem.sprite = takeSpriteFrom.sprite;
        HeldItem.enabled = true;
    }

    public void ClearHeldItem() 
    {
        HeldItem.enabled = false;
    }

    public void SetFeathers(int cnt)
    {
        for (int i = 0; i < Feathers.Count; i++)
        {
            if (i < cnt)
                Feathers[i].SetActive(true);
            else 
                Feathers[i].SetActive(false);
        }
    }

    private void SetSanityColor(Color color)
    {
        foreach (GameObject obj in SanityBarLevels)
        {
            Image img = obj.GetComponent<Image>();
            img.color = color;
        }
    }
}
