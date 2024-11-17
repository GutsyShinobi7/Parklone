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
    private void Start()
    {
        cloneHolder = null;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && canPressCAgain && !cloneButtonPressed)
        {
            StartCoroutine(InstantiateClone());
            cloneButtonPressed = true;
            canPressCAgain = false;
            print(canPressCAgain);
            StartCoroutine(CanPressCAgain());

        }

        else if (Input.GetKeyDown(KeyCode.C) && canPressCAgain && cloneButtonPressed)
        {
            cloneHolder.GetComponent<PolygonCollider2D>().enabled = false;
            cloneHolder.GetComponent<PlayerMovement>().enabled = false;
            cloneHolder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            this.GetComponent<PolygonCollider2D>().enabled = true;
            GetComponent<PlayerMovement>().enabled = true;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            StartCoroutine(DestroyClone());
            cloneButtonPressed = false;
            canPressCAgain = false;
            print(canPressCAgain);
            StartCoroutine(CanPressCAgain());

        }

        if (cloneButtonPressed && cloneHolder != null && GetComponent<PlayerMovement>().GetIsGrounded() && GetComponent<Rigidbody2D>().velocity.y == 0)
        {

            this.GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            
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

}