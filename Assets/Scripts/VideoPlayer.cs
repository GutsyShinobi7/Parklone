using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer; // Reference to the VideoPlayer

    private bool videoPlayedOnce = false;
    private void Awake() {
        videoPlayer.enabled = false;
    }
    private void Update()
    {

            
            if(GameObject.FindGameObjectWithTag("Player").GetComponent<SwitchManager>().BothSwitchesTriggered() && !videoPlayedOnce){
                videoPlayedOnce = true;
                videoPlayer.enabled = true;
                print("Video Triggered");
                videoPlayer.Play();
            } // Start the video
        
    }
    
}