using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // Include for scene management
using TMPro;
using System.IO;
public class FallingPlatform : MonoBehaviour
{


    [Header("Collider")]
    [SerializeField] private BoxCollider2D blinkingCollider;

    [Header("Game Object Attached To")]
    [SerializeField] private GameObject attachedGameObject;

    [Header("Platform Variabels")]

    [SerializeField] private float blinkSpeed = 0.5f;
    [SerializeField] private float fallWaitTIme = 2f;


    private bool active = true;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(InititatePlatformFall());
    }

    private IEnumerator InititatePlatformFall()
    {

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PolygonCollider2D>().IsTouching(GetComponent<BoxCollider2D>()))
        {
            StartCoroutine(Blink());
            yield return new WaitForSeconds(fallWaitTIme);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            blinkingCollider.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }

    }

    private IEnumerator Blink()
    {

        if (active)
        {
            yield return new WaitForSeconds(blinkSpeed);
            print("Gameobject active state set to false");
            foreach (SpriteRenderer spriteRenderer in attachedGameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.enabled = false;
            }
            active = false;
        }
        else if (!active)
        {
            yield return new WaitForSeconds(blinkSpeed);
            print("Gameobject active state set to true");
            foreach (SpriteRenderer spriteRenderer in attachedGameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.enabled = true;
            }
            active = true;
        }


    }
}
