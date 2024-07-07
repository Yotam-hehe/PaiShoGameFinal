using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCircularCameraMovement : MonoBehaviour
{
    public Transform target; // Target
    public float smoothSpeed = 2.0f; // Smoothness of movement
    public float radius = 5.0f; // Radius of the circular path
    public float angularSpeed = 1.0f; // Angular speed of the circular movement

    private Vector3 centerPosition; // Center position of the circular path
    private float currentAngle = 0.0f; // Current angle around the circle

    void Start()
    {
        //Calculate the center position of the circular path
        centerPosition = target.position;
    }

    void Update()
    {
        //Calculate the target position along the circular path
        Vector3 targetPosition = centerPosition + new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)) * radius;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Update the angle for the next frame
        currentAngle += angularSpeed * Time.deltaTime;

        // If the angle is more then 360 degrees, reset it
        if (currentAngle >= 360.0f)
        {
            currentAngle -= 360.0f;
        }
    }
}
