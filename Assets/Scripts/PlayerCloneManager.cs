using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerCloneManager : MonoBehaviour
{
    [SerializeField] private GameObject clonePrefab; // Assign the clone prefab in the Inspector



    private GameObject cloneHolder;

    private int existingClones;

    private void Awake()
    {
        existingClones = 0;
        cloneHolder = null;
    }
    
    private void Update()
    {
        print("Existing Clones: " + existingClones);
        if (Input.GetKeyDown(KeyCode.C))
        {

            if (existingClones == 0)
            {
                if (cloneHolder == null)
                {
                    InstantiateClone();
                }
            }
            else if (existingClones == 1)
            {
                StartCoroutine(PlayAnimation());
                //DestroyClone();

            }

        }
    }

    private void InstantiateClone()
    {
        cloneHolder = Instantiate(clonePrefab, transform.position + (Vector3.right * 2), transform.rotation);
        cloneHolder.GetComponent<Animator>().SetTrigger("jumpTrigger");
        existingClones++;
    }


    private void DestroyClone()
    {

        Destroy(cloneHolder);
    }


    private IEnumerator PlayAnimation()
    {
        print("PlayAnimationAndDestroy accessed");
        // Play the animation (assuming you have a trigger called "PlayAnimation")
        cloneHolder.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        cloneHolder.GetComponent<PlayerMovement>().enabled = false;
        cloneHolder.GetComponent<BoxCollider2D>().enabled = false;
        cloneHolder.GetComponent<Animator>().Play("Squirrel_Poof");


        // Wait for the animation to finish
        existingClones--;
        yield return new WaitForSeconds(cloneHolder.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        // Destroy the clone
        DestroyClone();

    }

    public bool cloneCreated()
    {
        return existingClones == 0;
    }


    public void disableClonePlayerMovement()
    {
        cloneHolder.GetComponent<PlayerMovement>().setActiveState(false);
    }

    public void enableClonePlayerMovement()
    {
        cloneHolder.GetComponent<PlayerMovement>().setActiveState(true);
        cloneHolder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        cloneHolder.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void freezeRigidbody()
    {
        cloneHolder.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void unFreezeRigidbody()
    {
        cloneHolder.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

    public Vector3 getClonePosition()
    {
        return cloneHolder.transform.position;
    }


}