using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    [SerializeField]
    private float HoverAmplitude = 0.2f;
    [SerializeField]
    private float HoverSpeed = 1.0f;
    [SerializeField]
    private GameObject Platform;

    void Start() {
        Platform.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float inverseSpeed = 1 / HoverSpeed;
        float yOffset = Mathf.Sin(Time.realtimeSinceStartup * inverseSpeed) * HoverAmplitude;
        Platform.transform.localPosition = new Vector3(0, yOffset, 0);;        
    }
}
