// Based on https://www.youtube.com/watch?v=KKGdDBFcu0Q from Dawnosaur

using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 6.5f;
    [SerializeField] private float timeToApex = 0.5f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpCancelGravityMult = 3.5f;
    [SerializeField] private float jumpHangTimeThreshold = 0f;
    [SerializeField] private float jumpHangGravityMult = 1f;
    [SerializeField] private float fallGravityMult = 2f;
    [SerializeField] private float jumpInputBufferTime = 0.2f;
    [SerializeField] private float walkMaxSpeed = 7f;
    [SerializeField] private float walkAcceleration = 1f;
    [SerializeField] private float walkDecceleration = 7f;
    [SerializeField] private float runMaxSpeed = 11f;
    [SerializeField] private float runAcceleration = 2.5f;
    [SerializeField] private float runDecceleration = 11f;
    [SerializeField] private float accelInAir = 0.65f;
    [SerializeField] private float deccelInAir = 0.65f;
    [SerializeField] private float jumpHangAccelerationMult = 1.1f;
    [SerializeField] private float jumpHangMaxSpeedMult = 1.3f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private CircleCollider2D col;
    private Animator playerAnim;
    private SpriteRenderer playerSprite;

    private float gravityScale;
    private float jumpForce;
    private float runAccelAmount;
    private float runDeccelAmount;
    private float walkAccelAmount;
    private float walkDeccelAmount;

    private float lastOnGroundTime;
    private float lastPressedJumpTime;
    private bool isJumpCancel;
    private bool isJumpDescent;

    private float inputDirection = 0f;
    private bool inputJumping = false;
    private bool inputJumpCancel = false;

    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; } = false;

    public bool IsModifiedRunning => PlayerState.Instance.AlwaysRun ? !IsRunning : IsRunning;

    private float BaseAcceleration => IsModifiedRunning ? runAccelAmount : walkAccelAmount;
    private float BaseDecceleration => IsModifiedRunning ? runDeccelAmount : walkDeccelAmount;
    private float Acceleration => lastOnGroundTime > 0 ? BaseAcceleration : BaseAcceleration * accelInAir;
    private float Decceleration => lastOnGroundTime > 0 ? BaseDecceleration : BaseDecceleration * deccelInAir;

    private float MaxSpeed => IsModifiedRunning ? runMaxSpeed : walkMaxSpeed;

    public void Move(CallbackContext context)
    {
        if (context.started)
        {
            inputDirection = context.ReadValue<float>();
            if (Time.timeScale != 0)
                playerSprite.flipX = inputDirection < 0;
        }
        else if (context.canceled)
        {
            inputDirection = 0f;
        }
    }

    public void Jump(CallbackContext context)
    {
        if (context.started)
            inputJumping = true;
        else if (context.canceled)
            inputJumpCancel = true;
    }

    public void Run(CallbackContext context)
    {
        if (context.started)
            IsRunning = true;
        else if (context.canceled)
            IsRunning = false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        playerAnim = GetComponentInChildren<Animator>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();

        float gravityStrength = -(2 * jumpHeight) / (timeToApex * timeToApex);
        gravityScale = gravityStrength / Physics2D.gravity.y;
        jumpForce = Mathf.Abs(gravityStrength) * timeToApex;
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;
        walkAccelAmount = (50 * walkAcceleration) / walkMaxSpeed;
        walkDeccelAmount = (50 * walkDecceleration) / walkMaxSpeed;
    }

    private void Start()
    {
        rb.gravityScale = gravityScale;
        IsFacingRight = true;
    }

    private void Update()
    {
        lastOnGroundTime -= Time.deltaTime;
        lastPressedJumpTime -= Time.deltaTime;

        InputCheck();
        GroundedCheck();
        JumpCheck();
        GravityCheck();
    }

    private void FixedUpdate()
    {
        playerAnim.SetBool("Running", IsModifiedRunning && inputDirection != 0f);
        playerAnim.SetBool("Moving", inputDirection != 0f);

        float targetSpeed = inputDirection * MaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, 1f);

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Acceleration : Decceleration;

        if ((IsJumping || isJumpDescent) && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold)
        {
            accelRate *= jumpHangAccelerationMult;
            targetSpeed *= jumpHangMaxSpeedMult;
        }

        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && lastOnGroundTime < 0)
            accelRate = 0;

        float speedDif = targetSpeed - rb.velocity.x;
        float movement = speedDif * accelRate;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void InputCheck()
    {
        if (inputDirection != 0f && inputDirection > 0f != IsFacingRight)
            IsFacingRight = !IsFacingRight;

        if (inputJumping)
        {
            lastPressedJumpTime = jumpInputBufferTime;
            inputJumping = false;
        }

        if (inputJumpCancel)
        {
            if (IsJumping && rb.velocity.y > 0)
                isJumpCancel = true;
            inputJumpCancel = false;
        }
    }

    private void GroundedCheck()
    {
        Collider2D hit = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + col.offset.y - col.radius - 0.1f), new Vector2(0.49f, 0.03f), 0, groundLayer);

        if (hit)
        {
            if (!IsJumping)
                lastOnGroundTime = coyoteTime;

            playerAnim.SetBool("Grounded", true);
        }
        else
        {
            playerAnim.SetBool("Grounded", false);
        }
    }

    private void JumpCheck()
    {
        if (IsJumping && rb.velocity.y < 0)
        {
            IsJumping = false;
            isJumpDescent = true;
            playerAnim.SetTrigger("JumpApex");
        }

        if (lastOnGroundTime > 0 && !IsJumping)
        {
            isJumpCancel = false;

            if (!IsJumping)
                isJumpDescent = false;
        }

        if ((lastOnGroundTime > 0 && !IsJumping) && lastPressedJumpTime > 0)
        {
            IsJumping = true;
            isJumpCancel = false;
            isJumpDescent = false;
            lastPressedJumpTime = 0;
            lastOnGroundTime = 0;

            float force = jumpForce;
            if (rb.velocity.y < 0)
                force -= rb.velocity.y;

            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            playerAnim.SetTrigger("Jump");
        }
    }

    private void GravityCheck()
    {
        if (isJumpCancel)
            rb.gravityScale = gravityScale * jumpCancelGravityMult;
        else if ((IsJumping || isJumpDescent) && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold)
            rb.gravityScale = gravityScale * jumpHangGravityMult;
        else if (rb.velocity.y < 0)
            rb.gravityScale = gravityScale * fallGravityMult;
        else
            rb.gravityScale = gravityScale;
    }
}
