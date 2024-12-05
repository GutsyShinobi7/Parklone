using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;


public class AudioController : MonoBehaviour
{

    public static AudioController instance { get; private set; }
    private AudioSource audioSource;

    [SerializeField] private AudioClip switchTriggeredMusic;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    
        print("Any switch triggered: " + GameObject.FindGameObjectWithTag("Player").GetComponent<SwitchManager>().AnySwitchTriggered());
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<SwitchManager>().AnySwitchTriggered())
        {
            if (audioSource.clip != switchTriggeredMusic) // Only change if it's not already set
            {
                audioSource.clip = switchTriggeredMusic;
                audioSource.loop = true; // Ensure loop is enabled
                audioSource.Play();
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }


}
