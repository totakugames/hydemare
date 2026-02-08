using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum EState 
    {
        Idle = 0,
        MoveTo = 1,
        Hunt = 2,
    }
    private EState Mode = EState.Idle;

    [SerializeField]
    private float MaxIdleTime = 2.5f;
    [SerializeField]
    private float MinIdleTime = 1.0f;
    private float IdleTimer;

    [SerializeField]
    private GameObject IdleBoxLimiterLeft;
    [SerializeField]
    private GameObject IdleBoxLimiterRight;
    private Vector3 MoveToTarget;
    [SerializeField]
    private float MovementSpeed = 1.0f;


    [SerializeField]
    private float AttackFrequency = 1.0f;
    private float AttackTimer;
    [SerializeField]
    private float AttackRange = 0.2f;
    [SerializeField]
    private float AttackDamage = 5.0f;
    
    [SerializeField]
    private Collider2D VisionColliderFront; // Needs to be isTrigger 
    [SerializeField]
    private Collider2D VisionColliderBack;

    [SerializeField]
    private bool CanFly = false;

    private bool FacesLeft = false;

    private GameObject Player;
    private SpriteRenderer Visual;
    private Animator Anim;
    private float OriginalVisualPlaybackSpeed;
    private Rigidbody2D RB;

    [SerializeField]
    private bool Debugging = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IdleTimer = NewIdleTime();
        AttackTimer = AttackFrequency;
        Player = GameObject.Find("Player");
        RB = GetComponent<Rigidbody2D>();
        GameObject visualObj = transform.GetChild(0).gameObject;
        Visual = visualObj.GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
        OriginalVisualPlaybackSpeed = Anim.speed;
    }

    void Update()
    {
        Visual.flipX = !FacesLeft;

        if (!CanFly) 
        {
            if (Mode == EState.Idle)
                Anim.speed = 0;
            else
                Anim.speed = OriginalVisualPlaybackSpeed;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dT = Time.deltaTime;
        AttackTimer -= dT;

        FacesLeft = MoveToTarget.x < transform.position.x;

        if (SeesPlayer())
        {
            Mode = EState.Hunt;
        }

        if (Debugging)
            Debug.Log(Mode);

        switch(Mode) 
        {
            case EState.Idle:
                IdleTimer -= dT;
                if (IdleTimer < 0)
                {
                    IdleTimer = NewIdleTime();
                    GenerateIdleTarget();
                    Mode = EState.MoveTo;
                }    
                break;
            case EState.MoveTo:
                if (CanFly)
                    Fly();
                else
                    Move();

                //Debug.Log(DistanceTo(MoveToTarget));
                if (DistanceTo(MoveToTarget) < 0.5f) 
                {
                    Mode = EState.Idle;
                    RB.linearVelocity = new Vector2(0, 0);
                }
                break;
            case EState.Hunt:
                if (SeesPlayerBidir())
                {
                    MoveToTarget = Player.transform.position;
                    if (DistanceTo(Player) <= AttackRange && AttackTimer < 0) 
                    {
                        Player.GetComponent<PlayerController>().DealDamage(AttackDamage);
                        AttackTimer = AttackFrequency;
                    }

                    if (CanFly)
                        Fly();
                    else
                        Move();
                    break;
                }
                else 
                {
                    Vector3 halfway = (IdleBoxLimiterRight.transform.position - IdleBoxLimiterLeft.transform.position) / 2;
                    MoveToTarget = IdleBoxLimiterLeft.transform.position + halfway;
                    if (!CanFly)
                        MoveToTarget.y = transform.position.y;
                    Mode = EState.MoveTo;

                    if (CanFly)
                        Fly();
                    else
                        Move();
                    break;
                }
                Mode = EState.MoveTo;   // Move to last known player position
                break;
        } 
    }

    float NewIdleTime() 
    {
        return Random.Range(MinIdleTime, MaxIdleTime);
    }

    void GenerateIdleTarget() 
    {
        float newX = Random.Range(  IdleBoxLimiterLeft.transform.position.x, 
                                        IdleBoxLimiterRight.transform.position.x);
        float newY = transform.position.y;

        if (CanFly) 
        {
            newY = Random.Range(  IdleBoxLimiterLeft.transform.position.y, 
                                        IdleBoxLimiterRight.transform.position.y);
        }

        MoveToTarget = new Vector3(newX, newY, transform.position.z);
    }

    // Need this in case player walks through enemy, so he doesn't loose him
    bool SeesPlayerBidir() 
    {
        if (VisionColliderFront.IsTouching(Player.GetComponent<Collider2D>()))
            return true;
        if (VisionColliderBack.IsTouching(Player.GetComponent<Collider2D>()))
            return true;
        return false;
    }

    bool SeesPlayer()
    {
        if (FacesLeft)
            if (VisionColliderBack.IsTouching(Player.GetComponent<Collider2D>()))
                return true;
        else
            if (VisionColliderFront.IsTouching(Player.GetComponent<Collider2D>()))
                return true;
        return false;
    }

    float DistanceTo(GameObject obj)
    {
        return (obj.transform.position - transform.position).magnitude;
    }
    float DistanceTo(Vector3 target)
    {
        return (target - transform.position).magnitude;
    }

    void Move() 
    {
        float dir = FacesLeft ? -1 : 1;
        float impulseX = dir * MovementSpeed;
        RB.linearVelocity = new Vector2(impulseX, 0);
    }
    
    void Fly()
    {
        float dir = FacesLeft ? -1 : 1;
        float impulseX = dir * MovementSpeed;
        float dirY = Mathf.Sign(MoveToTarget.y - transform.position.y);
        float impulseY = dirY * MovementSpeed;
        RB.linearVelocity = new Vector2(impulseX, impulseY);
        Debug.Log(impulseY + " Y");
    }
}
