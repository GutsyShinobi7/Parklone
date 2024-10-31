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

    private bool isFacingRight = true;

    private Rigidbody2D rb;
    private Animator playerAnimator;

    private PlayerCloneManager playerCloneManager;

    private BoxCollider2D boxCollider;

    private bool isWallSliding;

    private bool isWallJumping;

    private float wallJumpingTime = 0.2f;

    private float wallJumpingCounter;

    private float wallJumpingDirection;

    private float wallJumpingDuration = 0.4f;

    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 13f);

    private bool isActiveGameObject;

    private bool switchButtonPressed;

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

            horizontal = Input.GetAxisRaw("Horizontal");

            playerAnimator.SetBool("isRunning", horizontal != 0);
            playerAnimator.SetBool("isGrounded", IsGrounded());
            playerAnimator.SetBool("isOnWall", IsTouchingWall());

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                playerAnimator.SetTrigger("jumpTrigger");
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
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

                if (Input.GetKeyDown(KeyCode.RightShift))
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


}