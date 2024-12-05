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

    private bool bothSwitchesTriggered = false;

    [SerializeField] private Sprite crankUp;
    [SerializeField] private Sprite crankDown;

    [SerializeField] private AudioClip switchToggleSFX;


    [SerializeField] private GameObject objectiveText;

    private bool switchTriggeredFirstTime = false;
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
                if (!cloneOnSwitch2)
                {
                    AudioController.instance.PlaySound(switchToggleSFX);
                }
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
                if (!cloneOnSwitch1)
                {
                    AudioController.instance.PlaySound(switchToggleSFX);
                }
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
                if (!playerOnSwitch1)
                {
                    AudioController.instance.PlaySound(switchToggleSFX);
                }
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
                if (!playerOnSwitch2)
                {
                    AudioController.instance.PlaySound(switchToggleSFX);
                }
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
            bothSwitchesTriggered = true;
            print("Both player and clone have activated both switches!");
            // Trigger the desired action or event here
            objectiveText.SetActive(true);
            GetComponent<PlayerMovement>().setActiveState(false);
            GameObject.FindGameObjectWithTag("Clone").GetComponent<PlayerMovement>().setActiveState(false);
            Time.timeScale = 0f;
        }

        if (playerOnSwitch1 || playerOnSwitch2 || cloneOnSwitch1 || cloneOnSwitch2)
        {
            print("Switch triggered at least once!");
            switchTriggeredFirstTime = true;
        }
    }

    public bool BothSwitchesTriggered()
    {
        return bothSwitchesTriggered;
    }

    public bool AnySwitchTriggered()
    {
        return switchTriggeredFirstTime;
    }

}