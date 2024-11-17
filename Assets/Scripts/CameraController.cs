using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 offset = new Vector3(0, 0, -10f);
    private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    private float backgroundYLength;
    private float cameraYLength;

    [SerializeField] private GameObject backgroundImage;
    // Start is called before the first frame update

    private Vector3 targetPosition;
    [SerializeField] private Transform player;

    private void Start()
    {
        backgroundYLength = backgroundImage.GetComponent<SpriteRenderer>().bounds.size.y;
        cameraYLength = GetComponent<Camera>().orthographicSize * 2;
    }
    // Update is called once per frame
    void Update()
    {
        float backgroundImageYUpperBound = backgroundImage.transform.position.y + (backgroundYLength / 2);
        float backgroundImageYLowerBound = backgroundImage.transform.position.y - (backgroundYLength / 2);

       

        Transform activeGameObjectTransform = player;
            if (player.GetComponent<PlayerCloneManager>().DoesCloneExist())
            {
               activeGameObjectTransform = GameObject.FindWithTag("Clone").transform;
            }

         targetPosition = new Vector3(activeGameObjectTransform.position.x, Mathf.Clamp(activeGameObjectTransform.position.y, backgroundImageYLowerBound + 1 + cameraYLength / 2, backgroundImageYUpperBound - 1 - cameraYLength / 2), activeGameObjectTransform.position.z) + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

