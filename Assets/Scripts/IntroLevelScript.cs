using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLevelScript : MonoBehaviour
{

    [Header("Movement Keys")]
    [SerializeField] private GameObject leftKey;
    [SerializeField] private Sprite leftKeyDownSprite;
    [SerializeField] private Sprite leftKeyUpSprite;
    [SerializeField] private GameObject rightKey;
    [SerializeField] private Sprite rightKeyDownSprite;
    [SerializeField] private Sprite rightKeyUpSprite;
    [SerializeField] private GameObject A;
    [SerializeField] private Sprite AKeyDownSprite;
    [SerializeField] private Sprite AKeyUpSprite;
    [SerializeField] private GameObject D;
    [SerializeField] private Sprite DKeyDownSprite;
    [SerializeField] private Sprite DKeyUpSprite;

    [Header("Additional Keys")]
    [SerializeField] private GameObject spaceKey;
    [SerializeField] private Sprite spaceKeyDownSprite;
    [SerializeField] private Sprite spaceKeyUpSprite;

    [SerializeField] private GameObject rightShiftKey;
    [SerializeField] private Sprite rightShiftKeyDownSprite;
    [SerializeField] private Sprite rightShiftKeyUpSprite;

    [SerializeField] private GameObject FirstCheckPoint;
    private bool FirstCheckPointTriggered = false;

    [SerializeField] private GameObject SecondCheckPoint;
    private bool SecondCheckPointTriggered = false;

    [SerializeField] private GameObject ThirdCheckPoint;
    private bool ThirdCheckPointTriggered = false;

    [SerializeField] private GameObject FourthCheckPoint;
    private bool FourthCheckPointTriggered = false;

    private bool movementKeysPressed = false;
    private bool playFirstCheckpointDialogue = true;
    private bool playSecondCheckpointDialogue = true;
    private bool playThirdCheckpointDialogue = true;



    private bool cloneIntroTriggered = false;

    [SerializeField] private Material dissolveBoxMaterial;
    private float fade = 1f;

    // Start is called before the first frame update
    void Start()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
        DialogueController.instance.NewDialogueInstance("Press [LEFT]/[A] or [RIGHT]/[D] to move left or right.", "character_leo");
        DialogueController.instance.NewDialogueInstance("Go to the glowing area using the keys.", "character_leo");

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level 0")
        {
            // Handle movement input
            HandleMovementInput();

            if (GetComponent<Collider2D>().IsTouching(FirstCheckPoint.GetComponent<BoxCollider2D>()))
            {
                FirstCheckPointTriggered = true;

                if (playFirstCheckpointDialogue)
                {
                    playFirstCheckpointDialogue = false;
                    DialogueController.instance.NewDialogueInstance("Yay! You made it to the first checkpoint", "character_leo");
                    DialogueController.instance.NewDialogueInstance("Press Space to jump and Left/Right shift to dash while moving!", "character_leo");
                    DialogueController.instance.NewDialogueInstance("Use those moves to get to the next checkpoint!", "character_leo");


                }

            }

            if (FirstCheckPointTriggered)
            {

                FirstCheckPoint.GetComponent<SpriteRenderer>().material = dissolveBoxMaterial;

                fade -= Time.deltaTime;
                fade = Mathf.Clamp(fade, 0f, 1f);

                FirstCheckPoint.GetComponent<SpriteRenderer>().material.SetFloat("_Fade", fade); // Ensure this is called
            }

            if (fade == 0f && FirstCheckPointTriggered && !SecondCheckPointTriggered)
            {
                FirstCheckPoint.SetActive(false);
                fade = 1f;
            }


            if (!FirstCheckPointTriggered)
            {
                // Generate RGB values using a sine wave to create a smooth transition
                float r = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f; // Value between 0 and 1
                float g = Mathf.Sin(Time.time * 2f + 2f) * 0.5f + 0.5f;
                float b = Mathf.Sin(Time.time * 2f + 4f) * 0.5f + 0.5f;

                // Set the outline color for the material instance
                FirstCheckPoint.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(r, g, b));
            }

            if (GetComponent<Collider2D>().IsTouching(SecondCheckPoint.GetComponent<BoxCollider2D>()))
            {
                SecondCheckPointTriggered = true;

                if (playSecondCheckpointDialogue)
                {
                    playSecondCheckpointDialogue = false;
                    DialogueController.instance.NewDialogueInstance("Some serious moves there! Press C to Clone! ;)", "character_leo");

                }
            }

            if (SecondCheckPointTriggered && Input.GetKeyDown(KeyCode.C) && !cloneIntroTriggered)
            {
                cloneIntroTriggered = true;
                DialogueController.instance.NewDialogueInstance("BoOooOOOOOM! A CLONE!", "character_leo");
                DialogueController.instance.NewDialogueInstance("Your original will have a glowing outline", "character_leo");
                DialogueController.instance.NewDialogueInstance("Something special about this clone...", "character_leo");
                DialogueController.instance.NewDialogueInstance("The clone can double jump! Press Space while in air with the clone to double jump!", "character_leo");
                DialogueController.instance.NewDialogueInstance("Press C again to recall yourself to your clone's position!", "character_leo");
                DialogueController.instance.NewDialogueInstance("Too bad you're clone is dead :( mUAHAHAHA", "character_leo");
                DialogueController.instance.NewDialogueInstance("Use your clone to get to the next checkpoint!", "character_leo");

            }

            if (SecondCheckPointTriggered)
            {
                SecondCheckPoint.GetComponent<SpriteRenderer>().material = dissolveBoxMaterial;

                fade -= Time.deltaTime;
                fade = Mathf.Clamp(fade, 0f, 1f);

                SecondCheckPoint.GetComponent<SpriteRenderer>().material.SetFloat("_Fade", fade);
            }

            if (fade == 0f && FirstCheckPointTriggered && SecondCheckPointTriggered && !ThirdCheckPointTriggered)
            {
                SecondCheckPoint.SetActive(false);
                fade = 1f;
            }

            if (!SecondCheckPointTriggered)
            {
                // Generate RGB values using a sine wave to create a smooth transition
                float r = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f; // Value between 0 and 1
                float g = Mathf.Sin(Time.time * 2f + 2f) * 0.5f + 0.5f;
                float b = Mathf.Sin(Time.time * 2f + 4f) * 0.5f + 0.5f;

                // Set the outline color for the material instance
                SecondCheckPoint.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(r, g, b));
            }

            if (GetComponent<Collider2D>().IsTouching(ThirdCheckPoint.GetComponent<BoxCollider2D>()))
            {
                ThirdCheckPointTriggered = true;

                if (playThirdCheckpointDialogue)
                {
                    playThirdCheckpointDialogue = false;
                    DialogueController.instance.NewDialogueInstance("You're unstoppable! Keep going!", "character_leo");
                    DialogueController.instance.NewDialogueInstance("There are two switches. Use your clone to activate both switches to move to the next level!", "character_leo");
                    DialogueController.instance.NewDialogueInstance("Good luck!", "character_leo");
                }
            }

            if (ThirdCheckPointTriggered)
            {
                ThirdCheckPoint.GetComponent<SpriteRenderer>().material = dissolveBoxMaterial;

                fade -= Time.deltaTime;
                fade = Mathf.Clamp(fade, 0f, 1f);

                ThirdCheckPoint.GetComponent<SpriteRenderer>().material.SetFloat("_Fade", fade);

                GameObject.Find("Switch1").SetActive(true);
                GameObject.Find("Switch2").SetActive(true);
            }

            if (!ThirdCheckPointTriggered)
            {
                // Generate RGB values using a sine wave to create a smooth transition
                float r = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f; // Value between 0 and 1
                float g = Mathf.Sin(Time.time * 2f + 2f) * 0.5f + 0.5f;
                float b = Mathf.Sin(Time.time * 2f + 4f) * 0.5f + 0.5f;

                // Set the outline color for the material instance
                ThirdCheckPoint.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(r, g, b));
            }
            {
                // Generate RGB values using a sine wave to create a smooth transition
                float r = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f; // Value between 0 and 1
                float g = Mathf.Sin(Time.time * 2f + 2f) * 0.5f + 0.5f;
                float b = Mathf.Sin(Time.time * 2f + 4f) * 0.5f + 0.5f;

                // Set the outline color for the material instance
                GameObject.Find("Switch1").GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(r, g, b));
                GameObject.Find("Switch2").GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(r, g, b));
            }

        }
    }



    private void HandleMovementInput()
    {
        // Handle key visuals
        leftKey.GetComponent<SpriteRenderer>().sprite = Input.GetKey(KeyCode.LeftArrow) ? leftKeyDownSprite : leftKeyUpSprite;
        rightKey.GetComponent<SpriteRenderer>().sprite = Input.GetKey(KeyCode.RightArrow) ? rightKeyDownSprite : rightKeyUpSprite;
        A.GetComponent<SpriteRenderer>().sprite = Input.GetKey(KeyCode.A) ? AKeyDownSprite : AKeyUpSprite;
        D.GetComponent<SpriteRenderer>().sprite = Input.GetKey(KeyCode.D) ? DKeyDownSprite : DKeyUpSprite;

        spaceKey.GetComponent<SpriteRenderer>().sprite = Input.GetKey(KeyCode.Space) ? spaceKeyDownSprite : spaceKeyUpSprite;
        rightShiftKey.GetComponent<SpriteRenderer>().sprite = (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) ? rightShiftKeyDownSprite : rightShiftKeyUpSprite;
    }


    // Helper functions for other checkpoint triggers
    public bool WasFirstCheckPointTriggered()
    {
        return FirstCheckPointTriggered;
    }

    public bool WasSecondCheckPointTriggered()
    {
        return SecondCheckPointTriggered;
    }

    public bool WasThirdCheckPointTriggered()
    {
        return ThirdCheckPointTriggered;
    }

    public bool WasFourthCheckPointTriggered()
    {
        return FourthCheckPointTriggered;
    }
}