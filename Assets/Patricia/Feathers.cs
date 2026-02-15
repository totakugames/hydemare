using UnityEngine;

public class Feathers
{
    public int maxFeathers;
    public int currentFeathers = 0;

    private HudManager HM;

    public Feathers(int max) {
        maxFeathers = max;
        GameObject obj = GameObject.Find("Hud");
        HM = obj.GetComponent<HudManager>();
    }

    public void CollectFeather() {
        currentFeathers++;
        HM.SetFeathers(currentFeathers);
    }
}
