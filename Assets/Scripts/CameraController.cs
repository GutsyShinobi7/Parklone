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

    [SerializeField] private Transform player;

    private float originalOrthographicSize; // Store the original orthographic size
    private Vector3 targetPosition;

    private void Start()
    {
        // Store the original orthographic size
        originalOrthographicSize = GetComponent<Camera>().orthographicSize;

        // Adjust the camera size based on the screen resolution
        AdjustCameraSize();

        backgroundYLength = backgroundImage.GetComponent<SpriteRenderer>().bounds.size.y;
        cameraYLength = GetComponent<Camera>().orthographicSize * 2;
    }

    private void Update()
    {
        float backgroundImageYUpperBound = backgroundImage.transform.position.y + (backgroundYLength / 2);
        float backgroundImageYLowerBound = backgroundImage.transform.position.y - (backgroundYLength / 2);

        Transform activeGameObjectTransform = player;
        if (player.GetComponent<PlayerCloneManager>().DoesCloneExist())
        {
            if (!player.GetComponent<PlayerMovement>().getActiveState())
            {
                activeGameObjectTransform = GameObject.FindGameObjectWithTag("Clone").transform;
            }
            else
            {
                activeGameObjectTransform = player;
            }
        }

        targetPosition = new Vector3(
            activeGameObjectTransform.position.x,
            Mathf.Clamp(
                activeGameObjectTransform.position.y,
                backgroundImageYLowerBound + 1 + cameraYLength / 2,
                backgroundImageYUpperBound - 1 - cameraYLength / 2
            ),
            activeGameObjectTransform.position.z
        ) + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void AdjustCameraSize()
    {
        float targetAspect = 16f / 9f; // Target aspect ratio
        float windowAspect = (float)Screen.width / Screen.height;
        float scale = windowAspect / targetAspect;

        // Adjust the camera's orthographic size based on the scale
        GetComponent<Camera>().orthographicSize = originalOrthographicSize / scale;
    }
}