using System.Collections;
using UnityEngine;

public class PlayerCloneManager : MonoBehaviour
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float colorChangeSpeed = 2f; // Speed of the RGB effect

    [SerializeField] Material normalMaterial;
    [SerializeField] Material outlineMaterial;

    private Material outlineMaterialInstance; // Instance of the material for this specific object

    [Header("SFX")]
    [SerializeField] private AudioClip cloneSound;
    private GameObject cloneHolder;
    private bool canPressCAgain = true;
    private bool cloneButtonPressed = false;

    private bool switchButtonPressed = false;
    private Material cloneMaterial;
    private float fade = 1f;

    private bool isDissolved = false;
    private bool startDissolving = false;

    private void Start()
    {
        cloneHolder = null;
        // Create an instance of the outline material to ensure it's unique to this object
        outlineMaterialInstance = new Material(outlineMaterial);
        // Apply the instance material to the object's renderer
        GetComponent<SpriteRenderer>().material = normalMaterial;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && canPressCAgain && !cloneButtonPressed && GetComponent<IntroLevelScript>().WasFirstCheckPointTriggered() && GetComponent<IntroLevelScript>().WasSecondCheckPointTriggered())
        {
            AudioController.instance.PlaySound(cloneSound);
            StartCoroutine(InstantiateClone());
            disablePlayer();
            GetComponent<SpriteRenderer>().material = outlineMaterialInstance;
            cloneButtonPressed = true;
            canPressCAgain = false;
            StartCoroutine(CanPressCAgain());
        }

        if (Input.GetKeyDown(KeyCode.C) && canPressCAgain && cloneButtonPressed)
        {
            startDissolving = true;
            print("Start Dissolving set to true");
            disableClone();
            enablePlayer();
            GetComponent<SpriteRenderer>().material = normalMaterial;
            cloneHolder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            cloneHolder.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            cloneHolder.GetComponent<PolygonCollider2D>().enabled = false;
            cloneButtonPressed = false;
            canPressCAgain = false;
            StartCoroutine(CanPressCAgain());
            switchButtonPressed = false;
        }

        if (outlineMaterialInstance != null)
        {
            // Generate RGB values using a sine wave to create a smooth transition
            float r = Mathf.Sin(Time.time * colorChangeSpeed) * 0.5f + 0.5f; // Value between 0 and 1
            float g = Mathf.Sin(Time.time * colorChangeSpeed + 2f) * 0.5f + 0.5f;
            float b = Mathf.Sin(Time.time * colorChangeSpeed + 4f) * 0.5f + 0.5f;

            // Set the outline color for the material instance
            outlineMaterialInstance.SetColor("_Color", new Color(r, g, b));
        }

        Debug.Log("Current fade value: " + fade);
        print("Start Dissolving: " + startDissolving);
        if (startDissolving)
        {
            Debug.Log("Dissolving the clone..." + " Fade: " + fade);
            fade -= Time.deltaTime;
            fade = Mathf.Clamp(fade, 0f, 1f);

            cloneMaterial.SetFloat("_Fade", fade); // Ensure this is called

            if (fade <= 0)
            {
                fade = 1;
                Destroy(cloneHolder);
                startDissolving = false;
                transform.position = cloneHolder.transform.position;
                Debug.Log("Clone fully dissolved.");
            }
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
        cloneMaterial = cloneHolder.GetComponent<SpriteRenderer>().material; // This creates an instance
        cloneMaterial.SetFloat("_Fade", fade);
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