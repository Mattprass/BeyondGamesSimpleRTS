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

        if (Input.GetButton("Jump"))
            isFollowingPlayer = true;
        else
            isFollowingPlayer = false;
    }

    void LateUpdate()
    {
        if (isFollowingPlayer)
        {
            transform.position = target.position;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        }
        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    void HandleMovementInput()
    {
        if (Input.GetButton("CameraSprint"))
            movementSpeed = fastSpeed;
        else
            movementSpeed = normalSpeed;

        if (Input.GetAxis("Vertical") > 0f)
            newPosition += (transform.forward * movementSpeed);
        if (Input.GetAxis("Vertical") < 0f)
            newPosition += (transform.forward * -movementSpeed);
        if (Input.GetAxis("Horizontal") > 0f)
            newPosition += (transform.right * movementSpeed);
        if (Input.GetAxis("Horizontal") < 0f)
            newPosition += (transform.right * -movementSpeed);

        if (Input.mouseScrollDelta.y != 0f)
            newZoom += Input.mouseScrollDelta.y * zoomAmount;

    }
}
