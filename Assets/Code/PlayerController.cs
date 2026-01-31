using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    private InputAction TakeElevator;   // TakeElevator can also be stairs or whatever changes the vertical level
    private InputAction Jump;

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
                }
                else if (Interact.IsPressed())
                {
                    
                }
                else if (TakeElevator.IsPressed())
                {

                }
                break;
            case EPlayerState.SwitchingMask:
                break;
            case EPlayerState.Dead:
                break;
        }
    }

    void SetupInputSystem()
    {
        MoveLeft = InputSystem.actions.FindAction("MoveLeft");
        MoveRight = InputSystem.actions.FindAction("MoveRight");
        SwitchMask = InputSystem.actions.FindAction("SwitchMask");
        Interact = InputSystem.actions.FindAction("Interact");
        TakeElevator = InputSystem.actions.FindAction("TakeElevator");
        Jump = InputSystem.actions.FindAction("Jump");
    }

    void ProcessPlayerMovement()
    {
        float impulseX = 0;
        if (Mathf.Abs(RB.linearVelocity.x) < PlayerMaxSpeed) 
        {
            if (MoveLeft.IsPressed()) 
            {
                impulseX -= PlayerAcceleration;
            }
            else if (MoveRight.IsPressed()) 
            {
                impulseX += PlayerAcceleration;
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

        RB.AddForce(new Vector2(impulseX, impulseY), ForceMode2D.Impulse);
    }
}
