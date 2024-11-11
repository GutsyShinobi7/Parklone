using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 offset = new Vector3(0, 0, -10f);
    private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update

    private Vector3 targetPosition;
    [SerializeField] private Transform player;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

        if (player.GetComponent<PlayerMovement>().getActiveState())
        {
            targetPosition = player.position + offset;
        } else {
            targetPosition = player.GetComponent<PlayerCloneManager>().getClonePosition() + offset;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

