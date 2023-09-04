using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera mainCamera;

    [SerializeField]
    private float rotateSpeed = 0.075f;
    private float rotateVelocity;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit,
                Mathf.Infinity))
            {
                agent.SetDestination(hit.point);

                var lookrotation = Quaternion.LookRotation(hit.point - transform.position);
                var rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    lookrotation.eulerAngles.y,
                    ref rotateVelocity,
                    rotateSpeed * (Time.deltaTime * 5));
                transform.eulerAngles = new Vector3(0f, rotationY, 0f);
            }
        }
    }
}
