using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    public string objectName;
    [SerializeField]
    public Sprite objectIcon;
    [SerializeField]
    public Collect objectType;

    [SerializeField]    
    public float sanityGain = 10f;
}

public enum Collect 
{
    Feather,
    Carry,
    Climb
}

