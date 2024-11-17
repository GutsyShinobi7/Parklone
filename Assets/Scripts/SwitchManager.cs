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

    private GameObject cloneHolder; // Reference to the clone object

    [SerializeField] private GameObject objectiveText;

    private void Start()
    {
        objectiveText.SetActive(false);
    }

    private void Update()
    {

        if (GameObject.FindGameObjectWithTag("Clone") != null)
        {
            cloneHolder = GameObject.FindGameObjectWithTag("Clone");
            print("Clone Found");
        }
        else
        {
            cloneHolder = null;
            print("Clone Not Found");
        }

        if (cloneHolder != null)
        {
            if (cloneHolder.GetComponent<PolygonCollider2D>().IsTouchingLayers(LayerMask.GetMask("Switch1")))
            {
                print("Clone on Switch 1");
                GameObject.Find("Switch1").GetComponent<SpriteRenderer>().color = Color.green;
                cloneOnSwitch1 = true;
                cloneOnSwitch2 = false;
                if (GameObject.Find("Switch2").GetComponent<SpriteRenderer>().color == Color.green)
                {
                    GameObject.Find("Switch2").GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else if (cloneHolder.GetComponent<PolygonCollider2D>().IsTouchingLayers(LayerMask.GetMask("Switch2")))
            {
                print("Clone on Switch 2");
                GameObject.Find("Switch2").GetComponent<SpriteRenderer>().color = Color.green;
                cloneOnSwitch2 = true;
                cloneOnSwitch1 = false;
                if (GameObject.Find("Switch1").GetComponent<SpriteRenderer>().color == Color.green)
                {
                    GameObject.Find("Switch1").GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }

        if (GetComponent<PolygonCollider2D>().IsTouchingLayers(LayerMask.GetMask("Switch1")))
        {
            print("Player on Switch 1");
            GameObject.Find("Switch1").GetComponent<SpriteRenderer>().color = Color.green;
            playerOnSwitch1 = true;
            playerOnSwitch2 = false;
            if (GameObject.Find("Switch2").GetComponent<SpriteRenderer>().color == Color.green)
            {
                GameObject.Find("Switch2").GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        else if (GetComponent<PolygonCollider2D>().IsTouchingLayers(LayerMask.GetMask("Switch2")))
        {
            print("Player on Switch 2");
            GameObject.Find("Switch2").GetComponent<SpriteRenderer>().color = Color.green;
            playerOnSwitch2 = true;
            playerOnSwitch1 = false;
            if (GameObject.Find("Switch1").GetComponent<SpriteRenderer>().color == Color.green)
            {
                GameObject.Find("Switch1").GetComponent<SpriteRenderer>().color = Color.red;
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
            Time.timeScale = 0f;
        }
    }


}