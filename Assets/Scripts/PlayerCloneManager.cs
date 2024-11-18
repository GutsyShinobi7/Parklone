using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerCloneManager : MonoBehaviour
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private LayerMask groundLayer;


    private GameObject cloneHolder;
    private bool canPressCAgain = true;
    private bool cloneButtonPressed = false;

    private bool switchButtonPressed = false;
    private void Start()
    {
        cloneHolder = null;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && canPressCAgain && !cloneButtonPressed)
        {
            StartCoroutine(InstantiateClone());
            disablePlayer();
            cloneButtonPressed = true;
            canPressCAgain = false;
            print(canPressCAgain);
            StartCoroutine(CanPressCAgain());

        }

        else if (Input.GetKeyDown(KeyCode.C) && canPressCAgain && cloneButtonPressed)
        {
            disableClone();
            transform.position = cloneHolder.transform.position;
            enablePlayer();
            cloneHolder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            cloneHolder.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            cloneHolder.GetComponent<PolygonCollider2D>().enabled = false;
            StartCoroutine(DestroyClone());
            cloneButtonPressed = false;
            canPressCAgain = false;
            print("Can press C again: " + canPressCAgain);
            StartCoroutine(CanPressCAgain());
            switchButtonPressed = false;
        }

        if (cloneButtonPressed && cloneHolder != null && this.GetComponent<PlayerMovement>().GetFeetTouchingGround() && !switchButtonPressed)
        {
            print("Disabled player");
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<PolygonCollider2D>().enabled = false;
        }


        if (switchButtonPressed && cloneHolder != null && this.GetComponent<PlayerMovement>().GetFeetTouchingGround() && !this.GetComponent<PlayerMovement>().getActiveState())
        {
            print("Disabled player");
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<PolygonCollider2D>().enabled = false;
        }


        if (switchButtonPressed && cloneHolder != null && cloneHolder.GetComponent<PlayerMovement>().GetFeetTouchingGround() && !cloneHolder.GetComponent<PlayerMovement>().getActiveState())
        {
            print("Disabled clone");
            cloneHolder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            cloneHolder.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            cloneHolder.GetComponent<PolygonCollider2D>().enabled = false;
        }





        if (cloneHolder != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                switchButtonPressed = true;
                if (this.GetComponent<PlayerMovement>().getActiveState())
                {
                    print("Switching to clone");
                    enableClone();
                    disablePlayer();
                }
                else
                {
                    print("Switching to player");
                    enablePlayer();
                    disableClone();

                }
            }
        }



    }

    private IEnumerator InstantiateClone()
    {
        this.GetComponent<Animator>().SetTrigger("teleportTrigger");
        yield return new WaitForSeconds(0.5f);
        cloneHolder = Instantiate(clonePrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator DestroyClone()
    {
        cloneHolder.GetComponent<Animator>().SetTrigger("destroyTrigger");
        yield return new WaitForSeconds(0.5f);
        Destroy(cloneHolder);
    }

    private IEnumerator CanPressCAgain()
    {
        yield return new WaitForSeconds(2f);
        print("Setting canPressCAgain to true");
        canPressCAgain = true;
    }

    public bool DoesCloneExist()
    {
        return cloneHolder != null;
    }

    private void disablePlayer()
    {
        GetComponent<PlayerMovement>().setActiveState(false);
        GetComponent<Animator>().SetBool("isRunning", false);
    }

    private void enablePlayer()
    {
        GetComponent<PlayerMovement>().setActiveState(true);
        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }


    private void disableClone()
    {
        cloneHolder.GetComponent<PlayerMovement>().setActiveState(false);
        cloneHolder.GetComponent<Animator>().SetBool("isRunning", false);
    }
    private void enableClone()
    {
        cloneHolder.GetComponent<PlayerMovement>().setActiveState(true);
        cloneHolder.GetComponent<PolygonCollider2D>().enabled = true;
        cloneHolder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }




}