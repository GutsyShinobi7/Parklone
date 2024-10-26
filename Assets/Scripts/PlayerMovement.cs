using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D playerBody;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;


    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();

    }

    private void Update()
    { 

        playerBody.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, playerBody.velocity.y);

        if(Input.GetKey(KeyCode.Space)){

            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
        }    

    }
}
