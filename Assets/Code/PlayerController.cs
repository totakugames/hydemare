using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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

    private bool isNightmare = false;

    private bool canInteract = false;
    private Collectable interactable;
    private Collectable inventory;

    enum EPlayerState
    {
        Moving = 0,
        SwitchingMask = 1,
        Dead = 2,
    }
    private EPlayerState PlayerState = EPlayerState.Moving;

    private Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SetupInputSystem();

        playerSanity = new Sanity(PlayerMaxSanity, PlayerDrainSanity);
        playerFeathers = new Feathers(PlayerMaxFeathers);
    }

    void Update() {
        if(playerSanity.currentSanity <= 0){
            Debug.Log("Game Over");
        }
        if(playerFeathers.currentFeathers >= playerFeathers.maxFeathers){
            Debug.Log("You Won");
        }

        if(isNightmare) {
            playerSanity.DrainSanity(PlayerDrainSanity);
        }
    }

    void FixedUpdate()
    {
        switch(PlayerState) 
        {
            case EPlayerState.Moving:
                ProcessPlayerMovement();
                if (SwitchMask.IsPressed()) 
                {
                    // Freeze player for 1s and play mask switching animation
                    // Toggle visibility of items tagged with dark world

                    isNightmare = !isNightmare;
                    Debug.Log("Nightmare: " + isNightmare);
                }
                else if (Interact.IsPressed())
                {
                    if(canInteract) {
                        InteractWith();
                    }
                }
                break;
            case EPlayerState.SwitchingMask:
                break;
            case EPlayerState.Dead:
                break;
        }
    }

    void ChangeWorld() {
        isNightmare = !isNightmare;

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

    public void GainHealth(float gain) {
        playerFeathers.CollectFeather();
        playerSanity.GainSanity(gain);
    }

    public void DealDamage(float damage) {
        playerSanity.LoseSanity(damage);
    }

    public void CarryItem() {
        inventory = interactable;
        Debug.Log(inventory.objectName + " carried!");
    }

    public void DropItem() {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null)
        {
           interactable = collider.gameObject.GetComponent<Collectable>(); 
           canInteract = true;
           Debug.Log("Available Item: " + interactable.objectName);
        }
    }

    void OnTriggerExit2D(Collider2D collectable)
    {
        interactable = null;
        canInteract = false;

        Debug.Log("Available Item: None");
    }

    private void InteractWith() {
        switch(interactable.objectType)
        {
            case Collect.Climb: {   
                // todo
                break;
            }
            case Collect.Carry: {
                CarryItem();
                break;
            }
            case Collect.Feather: {
                GainHealth(interactable.sanityGain);
                break;
            }
            default: break;
        }
        Debug.Log(interactable.objectName + " collected!");

        Destroy(interactable.gameObject);
    }
}
