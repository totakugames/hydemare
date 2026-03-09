using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    public string objectName;
    [SerializeField]
    public Collect objectType;

    [SerializeField]    
    public float sanityGain = 0f;

    [Header("Story Settings (only for Feathers)")]
    [SerializeField]
    [TextArea(3, 10)] 
    public string storyText = "";
    
    void Start() {
        GameManager gm = GameObject.FindObjectOfType<GameManager>();

        if (!gm.IsRavenWorld)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }        
    }
}

public enum Collect 
{
    Feather,
    Carry
}

public class CollectableSnapshot 
{
    public CollectableSnapshot(GameObject obj) {
        collectable = obj.GetComponent<Collectable>();
        rotation = obj.transform.rotation;
        scale = obj.transform.lossyScale;
        sprite = obj.GetComponent<SpriteRenderer>().sprite;
    }
    
    public Collectable collectable;

    public Quaternion rotation;
    public Vector3 scale;
    public Sprite sprite;
}

