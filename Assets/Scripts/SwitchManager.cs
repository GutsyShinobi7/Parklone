using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchManager : MonoBehaviour
{
    // Flags to track if switches have been triggered by player and clone
    private bool playerOnSwitch1 = false;
    private bool playerOnSwitch2 = false;

    private bool cloneOnSwitch1 = false;
    private bool cloneOnSwitch2 = false;

    [SerializeField] private Sprite crankUp;
    [SerializeField] private Sprite crankDown;



    [SerializeField] private GameObject objectiveText;

    private void Start()
    {
        objectiveText.SetActive(false);
    }

    private void Update()
    {




        if (GetComponent<PlayerCloneManager>().DoesCloneExist())
        {
            if (GameObject.FindGameObjectWithTag("Clone").GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Switch2")))
            {
                GameObject.Find("Switch2").GetComponent<SpriteRenderer>().sprite = crankDown;
                print("Clone is on switch 2");
                cloneOnSwitch2 = true;
            }
            else
            {
                GameObject.Find("Switch2").GetComponent<SpriteRenderer>().sprite = crankUp;
                print("Clone is not on switch 2");
                cloneOnSwitch2 = false;
            }

            if (GameObject.FindGameObjectWithTag("Clone").GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Switch1")))
            {
                GameObject.Find("Switch1").GetComponent<SpriteRenderer>().sprite = crankDown;
                print("Clone is on switch 1");
                cloneOnSwitch1 = true;
            }
            else
            {
                GameObject.Find("Switch1").GetComponent<SpriteRenderer>().sprite = crankUp;
                print("Clone is not on switch 1");
                cloneOnSwitch1 = false;
            }
        }

        if (!cloneOnSwitch1)
        {
            if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Switch1")))
            {
                GameObject.Find("Switch1").GetComponent<SpriteRenderer>().sprite = crankDown;
                print("Player is on switch 1");
                playerOnSwitch1 = true;
            }
            else
            {
                GameObject.Find("Switch1").GetComponent<SpriteRenderer>().sprite = crankUp;
                print("Player is not on switch 1");
                playerOnSwitch1 = false;
            }
        }

        if (!cloneOnSwitch2)
        {
            if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Switch2")))
            {
                GameObject.Find("Switch2").GetComponent<SpriteRenderer>().sprite = crankDown;
                print("Player is on switch 2");
                playerOnSwitch2 = true;
            }
            else
            {
                GameObject.Find("Switch2").GetComponent<SpriteRenderer>().sprite = crankUp;
                print("Player is not on switch 2");
                playerOnSwitch2 = false;
            }
        }



        CheckBothSwitches();
    }



    private void CheckBothSwitches()
    {
        // Check if both player and clone are on both switches
        if ((playerOnSwitch1 && cloneOnSwitch2) || (playerOnSwitch2 && cloneOnSwitch1))
        {
            print("Both player and clone have activated both switches!");
            // Trigger the desired action or event here
            objectiveText.SetActive(true);
            GetComponent<PlayerMovement>().setActiveState(false);
            GameObject.FindGameObjectWithTag("Clone").GetComponent<PlayerMovement>().setActiveState(false);
            Time.timeScale = 0f;
        }
    }


}