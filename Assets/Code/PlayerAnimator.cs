using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "isWalking";
    private const string IS_JUMPING = "isJumping";
    private const string IS_GROUNDED = "isGrounded";

    [SerializeField] RuntimeAnimatorController swanController;
    [SerializeField] RuntimeAnimatorController ravenController;

    private Animator animator;
    private bool isSwanWorld = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = swanController;
    }

    public void SwitchWorld(bool toSwanWorld)
    {
        isSwanWorld = toSwanWorld;
        animator.runtimeAnimatorController = isSwanWorld ? swanController : ravenController;
    }

    public void SetWalking(bool walking)
    {
        animator.SetBool(IS_WALKING, walking);

        if (!walking)
        {
            GetComponent<PlayerSound>()?.StopWalkSound();
        }
    }

    public void SetJumping(bool jumping)
    {
        animator.SetBool(IS_JUMPING, jumping);
    }

    public void SetGrounded(bool grounded)
    {
        animator.SetBool(IS_GROUNDED, grounded);
    }
}