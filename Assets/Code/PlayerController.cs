using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject respawnPrefab;

    [SerializeField]
    private float PlayerMaxSanity = 100f;
    [SerializeField]
    private float PlayerDrainSanity = 1f;
    [SerializeField]
    private int PlayerMaxFeathers = 10;

    private Sanity playerSanity;
    private Feathers playerFeathers;

    [SerializeField]
    private float PlayerAcceleration = 1.0f;
    [SerializeField]
    private float PlayerMaxSpeed = 2.5f;
    [SerializeField]
    private float JumpMultiplier = 1.0f;

    private InputAction MoveLeft;
    private InputAction MoveRight;
    private InputAction SwitchMask;
    private InputAction Interact;
    private InputAction Jump;

    private bool isRavenWorld = false;

    private bool canInteractWithCollectable = false;
    private bool canInteractWithBase = false;
    private Collider2D interactable;

    private bool carrying = false;
    private CollectableSnapshot inventory;

    private float PickedUpTimeout = 0.5f;
    private float PickedUpTimer = 0;

    private float MaskingTime = 1.0f;
    private float MaskTimer;

    enum EPlayerState
    {
        Moving = 0,
        SwitchingMask = 1,
        Dead = 2,
    }
    private EPlayerState PlayerState = EPlayerState.Moving;

    private Rigidbody2D RB;
    private PlayerAnimator playerAnimator;

    [SerializeField]
    private GameManager GM;

    [SerializeField]
    private GameObject ES;
    private bool EndingScreenShown = false;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        SetupInputSystem();

        playerSanity = new Sanity(PlayerMaxSanity, PlayerDrainSanity);
        playerFeathers = new Feathers(PlayerMaxFeathers);

        MaskTimer = MaskingTime;
    }

    void Update()
    {
        if (!EndingScreenShown) {
            if (playerSanity.currentSanity <= 0)
            {
                ES.SetActive(true);
                ES.GetComponent<EndingUI>().SetEnding(false);
                Debug.Log("Game Over");
                EndingScreenShown = true;
            }
            if (playerFeathers.currentFeathers >= playerFeathers.maxFeathers)
            {
                ES.SetActive(true);
                ES.GetComponent<EndingUI>().SetEnding(true);
                Debug.Log("You Won");
                EndingScreenShown = true;
            }
        }
        

        if (isRavenWorld)
        {
            playerSanity.DrainSanity(PlayerDrainSanity);
        }

        UpdateAnimations();
    }

    void FixedUpdate()
    {
        PickedUpTimer -= Time.deltaTime;

        switch (PlayerState)
        {
            case EPlayerState.Moving:
                ProcessPlayerMovement();
                if (SwitchMask.IsPressed())
                {
                    isRavenWorld = !isRavenWorld;
                    MaskTimer = MaskingTime;
                    PlayerState = EPlayerState.SwitchingMask;
                }
                else if (Interact.IsPressed())
                {
                    if(canInteractWithCollectable && !carrying && PickedUpTimer < 0) {
                        PickedUpTimer = PickedUpTimeout;
                        InteractWith();
                    } else if(canInteractWithBase && PickedUpTimer < 0) {
                        if (carrying) {
                            PickedUpTimer = PickedUpTimeout;
                            InteractWithOther();
                        }
                    } else if(carrying && PickedUpTimer < 0) {
                        PickedUpTimer = PickedUpTimeout;
                        DropItem();
                    }
                }
                break;
            case EPlayerState.SwitchingMask:
                MaskTimer -= Time.deltaTime;
                if (MaskTimer < 0)
                {
                    GM.SwitchWorld(isRavenWorld);
                    playerAnimator.SwitchWorld(!isRavenWorld);
                    PlayerState = EPlayerState.Moving;
                }
                break;
            case EPlayerState.Dead:
                break;
        }
    }

    void UpdateAnimations()
    {
        if (playerAnimator == null) return;

        bool isMoving = MoveLeft.IsPressed() || MoveRight.IsPressed();
        playerAnimator.SetWalking(isMoving);

        if (MoveLeft.IsPressed())
        {
            playerAnimator.transform.localScale = new Vector3(-0.05f, 0.05f, 0.05f);
        }
        else if (MoveRight.IsPressed())
        {
            playerAnimator.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }

        bool isJumping = Jump.IsPressed() && Mathf.Abs(RB.linearVelocity.y) < 0.01f;
        playerAnimator.SetJumping(isJumping);

        bool isGrounded = Mathf.Abs(RB.linearVelocity.y) < 0.01f;
        playerAnimator.SetGrounded(isGrounded);
    }

    void ChangeWorld()
    {
        isRavenWorld = !isRavenWorld;

        Debug.Log("World Changed");
    }

    void SetupInputSystem()
    {
        MoveLeft = InputSystem.actions.FindAction("MoveLeft");
        MoveRight = InputSystem.actions.FindAction("MoveRight");
        SwitchMask = InputSystem.actions.FindAction("SwitchMask");
        Interact = InputSystem.actions.FindAction("Interact");
        Jump = InputSystem.actions.FindAction("Jump");
    }

    void ProcessPlayerMovement()
    {
        float velX = RB.linearVelocity.x;
        if (Mathf.Abs(RB.linearVelocity.x) < PlayerMaxSpeed)
        {
            if (MoveLeft.IsPressed())
            {
                velX -= PlayerAcceleration;
            }
            else if (MoveRight.IsPressed())
            {
                velX += PlayerAcceleration;
            }
        }

        float impulseY = 0;
        if (Jump.IsPressed())
        {
            if (RB.linearVelocity.y == 0)
            {
                impulseY = JumpMultiplier;
            }
        }

        RB.AddForce(new Vector2(0, impulseY), ForceMode2D.Impulse);
        RB.linearVelocity = new Vector2(velX, RB.linearVelocity.y);
    }

    public void GainHealth(float gain)
    {
        playerFeathers.CollectFeather();
        playerSanity.GainSanity(gain);
    }

    public void DealDamage(float damage)
    {
        playerSanity.LoseSanity(damage);
    }

    public void CarryItem()
    {
        inventory = new CollectableSnapshot(interactable.gameObject);
        canInteractWithCollectable = false;
        carrying = true;

        Debug.Log(inventory.collectable.objectName + " carried!");
    }

    public void DropItem()
    {
        GameObject spawn = Instantiate(
            respawnPrefab,
            transform.position,
            inventory.rotation
        );

        spawn.transform.localScale = inventory.scale;

        SpriteRenderer sr = spawn.GetComponent<SpriteRenderer>();
        sr.sprite = inventory.sprite;
        Collectable col = spawn.GetComponent<Collectable>();
        col.objectName = inventory.collectable.objectName;
        col.objectType = inventory.collectable.objectType;

        inventory = null;
        carrying = false;

        Debug.Log(col.objectName + " dropped!");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null)
        {
            interactable = collider;
            if(collider.gameObject.GetComponent<Collectable>()) {
                canInteractWithCollectable = true;
                Debug.Log("Available Item: " + interactable.gameObject.GetComponent<Collectable>().objectName);
            } else if(collider.gameObject.GetComponent<Base>()) {
                canInteractWithBase = true;
                Debug.Log("Available Base: " + interactable.gameObject.GetComponent<Base>().objectName);
            }    
        }
    }

    void OnTriggerExit2D(Collider2D collectable)
    {
        interactable = null;
        canInteractWithCollectable = false;
        canInteractWithBase = false;

        Debug.Log("Available Item: None");
    }

    private void InteractWith()
    {
        switch (interactable.gameObject.GetComponent<Collectable>().objectType)
        {
            case Collect.Climb:
                {
                    ClimbStairs(interactable.gameObject.GetComponent<Collectable>().escalatorTargetPosition);
                    break;
                }
            case Collect.Carry:
                {
                    if (!carrying)
                    {
                        CarryItem();
                    }
                    break;
                }
            case Collect.Feather:
                {
                    GainHealth(interactable.gameObject.GetComponent<Collectable>().sanityGain);
                    break;
                }
            default: break;
        }
        Destroy(interactable.gameObject);
        interactable = null;
    }

    private void InteractWithOther() {
        if(!interactable.gameObject.GetComponent<Base>().neededItems.Contains(inventory.collectable.objectName)) {
            Debug.Log("Thid doesn't fit here.");
            
            DropItem();
        } else {    
            Debug.Log(inventory.collectable.objectName + " used!");

            interactable.gameObject.GetComponent<Base>().ConsumeItem(inventory.collectable.objectName);
            inventory = null;
            carrying = false;
        }
    }

    private void ClimbStairs(Vector3 targetPosition) {
        transform.position = targetPosition;

        // wip
    }
}
