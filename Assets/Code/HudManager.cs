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

    public void Start() 
    {
        Blink = false;
        CurrentPercentage = 1.0f;
        HeldItem = HeldItemObj.GetComponent<Image>();
    }

    public void Update() 
    {
        if (Blink)
        {
            // get blink color
            // Get blink timer
        }
    }

    public void SetSanityBar(float percentage)
    {
        Blink = true;
        if (percentage < CurrentPercentage) 
            HealBlink = false;
        else
            HealBlink = true;
        
        int filling = (int)Mathf.Round((SanityBarLevels.Count - 1) * percentage);
        
        foreach (GameObject obj in SanityBarLevels)
        {
            obj.SetActive(false);
        }

        SanityBarLevels[filling].SetActive(true);
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
}
