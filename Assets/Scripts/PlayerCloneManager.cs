using System.Collections;
using UnityEngine;

public class PlayerCloneManager : MonoBehaviour
{
    [SerializeField] private GameObject clonePrefab; // Assign the clone prefab in the Inspector
    private GameObject currentClone; // Holds the current clone instance
    private PlayerMovement playerMovement;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleClone();
        } 

    }

    void ToggleClone()
    {
        if (currentClone == null)
        {
            // Spawn the clone at the player’s position
            currentClone = Instantiate(clonePrefab, transform.position + new Vector3(-Mathf.Sign(transform.localScale.x) + 2, 0, 0), transform.rotation);
            currentClone.GetComponent<Animator>().SetTrigger("jumpTrigger");
            currentClone.GetComponent<Rigidbody2D>().velocity = new Vector2(currentClone.GetComponent<Rigidbody2D>().velocity.x, 10f);

        }
        else
        {
            // Destroy the existing clone
            StartCoroutine(PlayAndDestroy(currentClone));
        }
    }

    IEnumerator PlayAndDestroy(GameObject clone)
    {
        
        Vector3 clonePosition =currentClone.transform.position;
        Animator animator = clone.GetComponent<Animator>();
        animator.Play("Squirrel_Poof");

        // Wait one frame to ensure the animation has started
        yield return null;

        // Now wait for the animation length before destroying
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(clone);
        currentClone = null;
        //recall player to clone position'
        yield return null;
        gameObject.transform.position = clonePosition;
    }

    public GameObject GetCurrentClone()
    {
        return currentClone;
    }
}