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

    public void Start() 
    {
        HeldItem = HeldItemObj.GetComponent<Image>();
    }

    public void SetSanityBar(float percentage)
    {
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
