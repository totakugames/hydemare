using UnityEngine;

public class Feathers
{
    public int maxFeathers;
    public int currentFeathers = 0;

    public Feathers(int max) {
        maxFeathers = max;
    }

    public void CollectFeather() {
        currentFeathers++;
    }
}
