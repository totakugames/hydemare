using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float PlayerSpeed = 1.0f;

    enum EPlayerState
    {
        Moving = 0,
        SwitchingMask = 1,
        TakingElevator = 2,
    }
    private EPlayerSate PlayerState = EPlayerSate.Moving;

    private RigidBody2D Body;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupInputSystem();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessPlayerInput();
    }

    private InputAction MoveLeft;
    private InputAction MoveRight;
    private InputAction SwitchMask;
    private InputAction Interact;
    private InputAction TakeElevator;
    private InputAction Jump;

    void SetupInputSystem()
    {
        MoveLeft = InputSystem.actions.FindAction("MoveLeft");
        MoveRight = InputSystem.actions.FindAction("MoveRight");
        SwitchMask = InputSystem.actions.FindAction("SwitchMask");
        Interact = InputSystem.actions.FindAction("Interact");
        TakeElevator = InputSystem.actions.FindAction("TakeElevator");
        Jump = InputSystem.actions.FindAction("Jump");
    }

    void ProcessPlayerInput()
    {
        float newPosX = transform.position.x;
        if (MoveLeft.IsPressed()) 
        {
            newPosX -= PlayerSpeed / 10.0f;
        }
        else if (MoveRight.IsPressed()) 
        {
            newPosX += PlayerSpeed / 10.0f;
        }
        transform.position = new Vector3(newPosX, 
                                        transform.position.y,
                                        transform.position.z);

        if (Switchmask.IsPressed()) 
        {
            
        }
        else if (Interact.IsPressed())
        {

        }
        else if (TakeElevator.IsPressed())
        {

        }
    }

    // Returns this if no elevator was found
    Elevator GetElevator() 
    {

    }
}
