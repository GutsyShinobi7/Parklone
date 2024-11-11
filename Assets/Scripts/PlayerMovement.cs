using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed;
    [SerializeField] private float jumpingPower;

    [SerializeField] private float fallingGravity;

    [SerializeField] private float jumpingGravity;

    [SerializeField] private int wallSlidingSpeed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private LayerMask bounceLayer;

    [SerializeField] private Transform wallCheck;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private Transform bounceCheck;

    [SerializeField] private TrailRenderer tr;

    private bool isFacingRight = true;

    private Rigidbody2D rb;
    private Animator playerAnimator;

    private PlayerCloneManager playerCloneManager;

    private BoxCollider2D boxCollider;


    private bool isWallSliding;

    private bool isWallJumping;

    private bool doubleJump;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.1f;

    private float jumpBufferCounter;

    private float wallJumpingTime = 0.1f;

    private float wallJumpingCounter;

    private float wallJumpingDirection;

    private float wallJumpingDuration = 0.4f;

    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 13f);

    private bool isActiveGameObject;

    private bool cloneButtonPressed;

    //teleportation variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    public bool canToggleClone = true;
    public float toggleCooldown = 1.0f;

    private bool cloneExists = false;


    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        if (tag == "Player")
        {
            playerCloneManager = GetComponent<PlayerCloneManager>();
        }
        else
        {
            playerCloneManager = null;
        }
        isActiveGameObject = true;
        cloneButtonPressed = false;


    }
    void Update()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerAnimator.SetBool("isRunning", horizontal != 0);
        playerAnimator.SetBool("isGrounded", IsGrounded());
        playerAnimator.SetBool("isOnWall", IsTouchingWall());

        if (isActiveGameObject)
        {

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
                playerAnimator.SetTrigger("jumpTrigger");
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                jumpBufferCounter = 0f;
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                coyoteTimeCounter = 0f;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            }

            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallingGravity;
            }

            if (IsGrounded())
            {
                rb.gravityScale = 3f;
            }

            WallSlide();
            WallJump();

            if (!isWallJumping)
            {
                Flip();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
            }

        }

        // Set player to static only when inactive, on the ground, and stationary
        if (!isActiveGameObject && rb.velocity.y == 0f && IsGrounded())
        {
            boxCollider.enabled = false;
            rb.bodyType = RigidbodyType2D.Static;
            playerAnimator.SetBool("isRunning", false);
        }

        // Toggle with key press
        if (Input.GetKeyDown(KeyCode.C) && canToggleClone)
        {

            ToggleCloneState();
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerCloneManager != null)
            {
                if (!playerCloneManager.cloneCreated())
                {
                    if (isActiveGameObject)
                    {
                        isActiveGameObject = false;
                        playerCloneManager.enableClonePlayerMovement();
                        playerCloneManager.unFreezeRigidbody();
                    }
                    else
                    {
                        print("Player is active game object");
                        isActiveGameObject = true;
                        rb.bodyType = RigidbodyType2D.Dynamic;
                        boxCollider.enabled = true;
                        playerCloneManager.disableClonePlayerMovement();
                        playerCloneManager.freezeRigidbody();
                        
                    }
                }
            }

        }





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

    void ToggleCloneState()
    {
        if (playerCloneManager.cloneCreated())
        {
            isActiveGameObject = false;
            rb.velocity = new Vector2(0, rb.velocity.y);

        }
        else
        {
            cloneExists = false;
            isActiveGameObject = true;
            boxCollider.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;

            // Start the cooldown only after destroying the clone
            canToggleClone = false;
            StartCoroutine(ResetToggleCooldown());
        }
    }

    // Coroutine to reset the cooldown
    private IEnumerator ResetToggleCooldown()
    {
        yield return new WaitForSeconds(toggleCooldown);
        canToggleClone = true;
    }

    public void setActiveGameObject(bool value)
    {
        isActiveGameObject = value;
    }

}