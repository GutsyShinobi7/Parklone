using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D playerBody;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;

    private bool grounded;

    private Animator playerAnimator;

    private Collider2D playerCollider;


    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();

        playerAnimator = GetComponent<Animator>();

        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            playerBody.velocity = new Vector2(horizontalInput * speed, playerBody.velocity.y);
        }

        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(3, 3, 3);

        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-3, 3, 3);

        }

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();

        }

        //set animation bool parameters here
        playerAnimator.SetBool("isRunning", horizontalInput != 0);
        playerAnimator.SetBool("isGrounded", grounded);

    }

    private void Jump()
    {
        playerBody.velocity = new Vector2(playerBody.velocity.x , jumpSpeed);
        playerAnimator.SetTrigger("jumpTrigger");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Ground")
        {
            grounded = true;

        }
    }
}
