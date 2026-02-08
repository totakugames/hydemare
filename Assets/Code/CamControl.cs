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
    private GameObject Background;
    private SpriteRenderer BackgroundSprite;

    private Vector2 CamPosMin;
    private Vector2 CamPosMax;

    private Camera MainCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(Player.transform.position.x,
                                        Player.transform.position.y,
                                        transform.position.z);
        BackgroundSprite = Background.GetComponent<SpriteRenderer>();
        MainCam = GetComponent<Camera>();
        
        Vector3 topLeftCorner = new Vector3(0, MainCam.pixelHeight, 0);
        Vector3 bottomRightCorner = new Vector3(MainCam.pixelWidth, 0, 0);

        Bounds bgBounds = BackgroundSprite.bounds;
        float camWidthHalf = (MainCam.ScreenToWorldPoint(bottomRightCorner).x - MainCam.ScreenToWorldPoint(topLeftCorner).x) / 2;
        CamPosMin.x = camWidthHalf + bgBounds.center.x - bgBounds.extents.x;
        CamPosMax.x = bgBounds.center.x + bgBounds.extents.x - camWidthHalf;

        float camHeightHalf = (MainCam.ScreenToWorldPoint(topLeftCorner).y - MainCam.ScreenToWorldPoint(bottomRightCorner).y) / 2;
        CamPosMin.y = bgBounds.center.y - bgBounds.extents.y + camHeightHalf;
        CamPosMax.y = bgBounds.center.y + bgBounds.extents.y - camHeightHalf;
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

        newX = Mathf.Clamp(newX, CamPosMin.x, CamPosMax.x);
        newY = Mathf.Clamp(newY, CamPosMin.y, CamPosMax.y);
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
