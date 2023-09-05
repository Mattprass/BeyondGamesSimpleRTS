using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float normalSpeed;
    [SerializeField]
    private float fastSpeed;
    [SerializeField]
    private float movementTime;
    [SerializeField]
    private Transform camTransform;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 zoomAmount;

    private float movementSpeed;
    private Vector3 newPosition;
    private Vector3 newZoom;
    private bool isFollowingPlayer;

    void Start()
    {
        newPosition = transform.position;
        newZoom = camTransform.localPosition;
    }

    void Update()
    {
        HandleMovementInput();

        //Holding space to follow the player
        if (Input.GetButton("FollowPlayer"))
            isFollowingPlayer = true;
        else
            isFollowingPlayer = false;
    }

    //Camera movement should be in LateUpdate to prevent stuttery movement
    void LateUpdate()
    {
        if (isFollowingPlayer)
            transform.position = target.position;
        else
            transform.position = Vector3.Lerp(transform.position, transform.position + newPosition, Time.deltaTime * movementTime);
        
        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    void HandleMovementInput()
    {
        //For the camera to move faster, hold shift
        if (Input.GetButton("CameraSprint"))
            movementSpeed = fastSpeed;
        else
            movementSpeed = normalSpeed;

        //Camera Movement
        newPosition = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))
            .normalized * movementSpeed;

        //Camera Zoom
        if (Input.mouseScrollDelta.y != 0f)
            newZoom += Input.mouseScrollDelta.y * zoomAmount;

    }
}
