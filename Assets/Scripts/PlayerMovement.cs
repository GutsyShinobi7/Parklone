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

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;

    private float jumpBufferCounter;

    private float wallJumpingTime = 0.2f;

    private float wallJumpingCounter;

    private float wallJumpingDirection;

    private float wallJumpingDuration = 0.4f;

    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 13f);

    private bool isActiveGameObject;

    private bool switchButtonPressed;

    //teleportation variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerCloneManager = GetComponent<PlayerCloneManager>();
        isActiveGameObject = true;
        switchButtonPressed = false;


    }
    void Update()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (isActiveGameObject)
        {

            if(isDashing){
                return;
            }

            horizontal = Input.GetAxisRaw("Horizontal");

            playerAnimator.SetBool("isRunning", horizontal != 0);
            playerAnimator.SetBool("isGrounded", IsGrounded());
            playerAnimator.SetBool("isOnWall", IsTouchingWall());

            if(IsGrounded()){
                coyoteTimeCounter = coyoteTime;
            } else {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if(Input.GetButtonDown("Jump")){
                jumpBufferCounter = jumpBufferTime;
            } else {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
            {
                playerAnimator.SetTrigger("jumpTrigger");
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                jumpBufferCounter = 0f;
            }


            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                playerAnimator.SetTrigger("jumpTrigger");
                rb.gravityScale = jumpingGravity;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.7f);
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

            if(Input.GetKeyDown(KeyCode.LeftShift) && canDash){
                StartCoroutine(Dash());
            }

        }

        //CLONE LOGIC
        if (playerCloneManager != null)
        {
            GameObject clone = playerCloneManager.GetCurrentClone();
            if (clone != null)
            {
                if (!switchButtonPressed)
                {
                    isActiveGameObject = false;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    print("Player is not active game object is being triggered in every update");
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    switchButtonPressed = true;

                    if (isActiveGameObject)
                    {
                        isActiveGameObject = false;
                        clone.GetComponent<PlayerMovement>().enabled = true;
                        rb.constraints = RigidbodyConstraints2D.FreezeAll;
                        clone.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        print("Player is not active game object is being triggered in every update");
                    }
                    else
                    {
                        isActiveGameObject = true;
                        clone.GetComponent<PlayerMovement>().enabled = false;
                        clone.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                        rb.constraints = RigidbodyConstraints2D.None;
                        print("Player is active game object is being triggered in every update");
                    }
                }
            }
            else
            {
                isActiveGameObject = true;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                ;
            }

        }


    }

    private void FixedUpdate()
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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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

    private IEnumerator Dash(){
        canDash = false;
        isDashing = true;
        if(IsGrounded() && playerAnimator.GetBool("isRunning")){
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


}