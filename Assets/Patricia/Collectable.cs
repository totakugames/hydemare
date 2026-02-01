using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    public string objectName;
    [SerializeField]
    public Collect objectType;

    [SerializeField]    
    public float sanityGain = 0f;

    [SerializeField]
    public Vector3 escalatorTargetPosition;
}

public enum Collect 
{
    Feather,
    Carry,
    Climb
}

public class CollectableSnapshot 
{
    public CollectableSnapshot(GameObject obj) {
        collectable = obj.GetComponent<Collectable>();
        rotation = obj.transform.rotation;
        scale = obj.transform.localScale;
        sprite = obj.GetComponent<SpriteRenderer>().sprite;
    }

    public Collectable collectable;

    public Quaternion rotation;
    public Vector3 scale;
    public Sprite sprite;
}

