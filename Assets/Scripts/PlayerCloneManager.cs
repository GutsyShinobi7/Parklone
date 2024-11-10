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

        if (Input.GetKeyDown(KeyCode.C))
        {
            print("C key pressed");
            print("ExistingClones: " + existingClones);
            if (existingClones == 0)
            {
                if (cloneHolder == null)
                {


                    InstantiateClone();
                    existingClones++;
                    print("Instantiated clone");
                }

            }
            else if (existingClones == 1)
            {
                StartCoroutine(PlayAnimation());
                //DestroyClone();
                print("Destroyed clone");

            }

        }
    }

    private void InstantiateClone()
    {
        cloneHolder = Instantiate(clonePrefab, transform.position + (Vector3.right * 2), transform.rotation);
        cloneHolder.GetComponent<Animator>().SetTrigger("jumpTrigger");
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
        cloneHolder.GetComponent<Animator>().Play("Squirrel_Poof");

        // Wait for the animation to finish
        existingClones--;
        yield return new WaitForSeconds(cloneHolder.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        // Destroy the clone
        DestroyClone();

    }

    public bool cloneExists()
    {
        return cloneHolder != null;
    }

    public void disableClonePlayerMovement(){
        GetComponent<PlayerMovement>().enabled = false;
    }

    public void enableClonePlayerMovement(){
        GetComponent<PlayerMovement>().enabled = true;
    }

    public void freezeRigidbody(){
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void unFreezeRigidbody(){
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }


}