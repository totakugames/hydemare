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

    [SerializeField]
    private float VerticalOffset = 0;

    [SerializeField]
    private GameObject Background;
    private SpriteRenderer BackgroundSprite;
    private float CamPosMinX;
    private float CamPosMaxX;

    private Camera MainCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(Player.transform.position.x,
                                        Player.transform.position.y,
                                        transform.position.z);
        BackgroundSprite = Background.GetComponent<SpriteRenderer>();
        MainCam = GetComponent<Camera>();
        
        Bounds bgBounds = BackgroundSprite.sprite.bounds;
        float camWidthHalf = (MainCam.ScreenToWorldPoint(new Vector3(MainCam.pixelWidth, 0, 0)).x - MainCam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x) / 2;
        CamPosMinX = camWidthHalf + bgBounds.center.x - bgBounds.extents.x;
        CamPosMaxX = bgBounds.center.x + bgBounds.extents.x - camWidthHalf;
    }

    // Update is called once per frame
    void FixedUpdate()
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

        newX = Mathf.Clamp(newX, CamPosMinX, CamPosMaxX);
        transform.position = new Vector3(newX, newY + VerticalOffset, transform.position.z);
    }
}
