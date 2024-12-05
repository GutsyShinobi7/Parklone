using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]

    [Header("Speed Variables")]
    private float horizontal;
    [SerializeField] private float speed;

    [SerializeField] private int wallSlidingSpeed;
    [SerializeField] private float jumpingPower;

    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 13f);


    [Header("Gravity Variables")]

    [SerializeField] private float fallingGravity;

    [SerializeField] private float jumpingGravity;

    [Header("Layer Variables")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private LayerMask bounceLayer;

    [Header("Check Variables")]
    [SerializeField] private Transform wallCheck;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private Transform feetContactPointCheck;

    [Header("Transform")]
    [SerializeField] private TrailRenderer tr;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSFX;

    [SerializeField] private AudioClip footStepsSFX;

    private bool isFacingRight = true;

    #region Component Variables
    private Rigidbody2D rb;
    private Animator playerAnimator;

    private PlayerCloneManager playerCloneManager;

    private PolygonCollider2D polygonCollider;

    #endregion

    #region Jump Variables

    private bool isWallSliding;

    private bool isWallJumping;

    private bool doubleJump = true;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.1f;

    private float jumpBufferCounter;

    private float wallJumpingTime = 0.1f;

    private float wallJumpingCounter;

    private float wallJumpingDirection;

    private float wallJumpingDuration = 0.4f;

    #endregion

    private bool isActiveGameObject;


    #region Teleport Variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    #endregion





    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();


        isActiveGameObject = true;



    }
    void Update()
    {
        if (Time.timeScale != 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerAnimator.SetBool("isGrounded", IsGrounded());
            playerAnimator.SetBool("isOnWall", IsTouchingWall());

        }

        if (isActiveGameObject)
        {
            playerAnimator.SetBool("isRunning", horizontal != 0);

            if (isDashing)
            {
                return;
            }

            horizontal = Input.GetAxisRaw("Horizontal");

            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f)
            {   
                AudioController.instance.PlaySound(jumpSFX);
                playerAnimator.SetTrigger("jumpTrigger");
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                jumpBufferCounter = 0f;
            }

            if (tag == "Clone" && Input.GetButtonDown("Jump") && !IsGrounded() && doubleJump)
            {
                playerAnimator.SetTrigger("jumpTrigger");
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                doubleJump = false;
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                coyoteTimeCounter = 0f;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.75f);
            }

            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallingGravity;
            }

            if (IsGrounded())
            {
                rb.gravityScale = 3f;
                doubleJump = true;
            }

            WallSlide();
            WallJump();

            if (!isWallJumping && IsGrounded())
            {
                if (Time.timeScale != 0)
                    Flip();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && horizontal != 0)
            {
                StartCoroutine(Dash());
            }

        }

        print("Feet touching ground: " + IsFeetTouchingGround());

    }



    private void FixedUpdate()
    {
        if (isActiveGameObject)
        {
            if (isDashing)
            {
                return;
            }
            if (!isWallJumping)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);
    }

    private bool IsTouchingWall()
    {

        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private bool IsFeetTouchingGround()
    {
        return Physics2D.OverlapCapsule(
       feetContactPointCheck.position,                        // Center position of the capsule
       new Vector2(0.75f, 0.005f),              // Size (width and height) of the capsule
       CapsuleDirection2D.Horizontal,                          // Capsule direction
       0f,                                                   // Rotation in degrees
       groundLayer                                            // Layer mask
   );
    }

    //for debugging
    private void OnDrawGizmos()
    {
        if (feetContactPointCheck != null)
        {
            Gizmos.color = Color.red; // Color of the capsule
            Gizmos.DrawWireCube(feetContactPointCheck.position, new Vector3(0.75f, 0.005f, 0)); // Approximate visualization
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.red; // Color of the capsule
            Gizmos.DrawWireCube(groundCheck.position, new Vector3(0.3f, 0.3f, 0)); // Approximate visualization
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.red; // Color of the capsule
            Gizmos.DrawWireCube(wallCheck.position, new Vector3(0.2f, 0.2f, 0)); // Approximate visualization
        }
    }

    private void WallSlide()
    {

        if (IsTouchingWall() && !IsGrounded() && horizontal != 0f)
        {
            playerAnimator.SetBool("isRunning", false);
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {

            isWallSliding = false;
        }


    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        if (IsGrounded() && playerAnimator.GetBool("isRunning"))
        {
            playerAnimator.SetBool("isRunning", true);
        }
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        playerAnimator.SetBool("isRunning", false);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }




    public void setActiveState(bool value)
    {
        isActiveGameObject = value;
    }
    public bool getActiveState()
    {
        return isActiveGameObject;
    }

    public bool GetIsGrounded()
    {
        return IsGrounded();
    }

    public bool GetFeetTouchingGround()
    {
        return IsFeetTouchingGround();
    }





}
