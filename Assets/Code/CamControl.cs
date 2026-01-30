using UnityEngine;

public class CamControl : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private int MaxDistanceY = 200;
    [SerializeField]
    private int MaxDistanceX = 300;
    [SerializeField]
    private float CamSpeedFactor = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Start()
    {
        transform.position = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float dY = Player.transform.position.y - transform.position.y;
        float dX = Player.transform.position.x - transform.position.x;

        float newY = transform.position.y;
        float newX = transform.position.x;

        if (Mathf.Abs(dY) > MaxDistanceY) 
        {
            float moveY = ((dY - Mathf.Sign(dY) * MaxDistanceY) / 10.0f) * CamSpeedFactor;
            newY += moveY;
        }
        if (Mathf.Abs(dX) > MaxDistanceX)
        {
            float moveX = ((dX - Mathf.Sign(dX) * MaxDistanceX) / 10.0f) * CamSpeedFactor;
            newX += moveX;
        }        

        transform.position = new Vector3(newX, newY, transform.position.z);
    }*/
}
