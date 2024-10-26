using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    private Animator animator;
    public RuntimeAnimatorController idleController;  // Assign in Inspector if needed
    public RuntimeAnimatorController jumpController;   // Assign in Inspector or by path
    public RuntimeAnimatorController runController;    // Assign in Inspector or by path

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = idleController; // Set initial controller
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Switch to the jump animation controller
            animator.runtimeAnimatorController = jumpController;
            animator.speed = 0f;

        } else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){

            // Switch to the run animation controller   
            animator.runtimeAnimatorController = runController;
            animator.speed = 1.0f;
        }
        else
        {
            // Switch back to the idle controller when not running
            animator.runtimeAnimatorController = idleController;
            animator.speed = 1.0f;
        }
    }
}