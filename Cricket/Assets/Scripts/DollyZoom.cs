using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyZoom : MonoBehaviour
{
    public CameraFollow CameraFollow;
    public Camera mainCamera;
    public Transform target; // The target object to focus on
    public float zoomSpeed = 1.0f; // Speed of zooming
    public float maxZoom = 10.0f; // Maximum FOV
    public float minZoom = 5.0f; // Minimum FOV
    public float defaultFOV; // Default field of view value

    void Start()
    {
        CameraFollow = GetComponent<CameraFollow>();
        mainCamera = GetComponent<Camera>();
        defaultFOV = mainCamera.fieldOfView;  // Default field of view value

    }
    void Update()
    {
        if (CameraFollow != null)
        {
            if (CameraFollow.isMove)
            {
                target = BallControllerScript.instance.ball.transform;
                // Calculate the distance between the camera and the target
                float distance = Vector3.Distance(mainCamera.transform.position, target.position);

                // Calculate the desired field of view based on the distance
                float desiredFOV = Mathf.Clamp(distance * zoomSpeed, minZoom, maxZoom);

                // Smoothly interpolate between the current FOV and the desired FOV
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, desiredFOV, Time.deltaTime);

                // Optionally, you can adjust the position of the camera to maintain the target in view
                // You can add more sophisticated camera movement logic here if needed
            }
        }
    }
    public void ResetZoom()
    {
        mainCamera.fieldOfView = defaultFOV;
    }
}
