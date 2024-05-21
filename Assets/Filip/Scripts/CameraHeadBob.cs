using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeadBob : MonoBehaviour
{
    // Head bob parameters for different states
    public float idleBobFrequency = 1.0f;
    public float idleBobHeight = 0.02f;
    public float idleBobSwayAngle = 0.3f;

    public float walkBobFrequency = 1.5f;
    public float walkBobHeight = 0.05f;
    public float walkBobSwayAngle = 0.5f;

    public float runBobFrequency = 2.5f;
    public float runBobHeight = 0.1f;
    public float runBobSwayAngle = 1.0f;

    private float timer = 0.0f;
    private Vector3 startPosition;
    private Quaternion startRotation;

    [SerializeField] private CharacterController playerExampleController;

    void Start()
    {
        // Store the initial position and rotation of the camera
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;

        // Get the CharacterController component
        //playerExampleController = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        // Determine the player's current speed
        float speed = playerExampleController.velocity.magnitude;

        // Set the bob parameters based on the player's speed
        float bobFrequency;
        float bobHeight;
        float bobSwayAngle;

        if (speed < 0.1f) // Idle
        {
            bobFrequency = idleBobFrequency;
            bobHeight = idleBobHeight;
            bobSwayAngle = idleBobSwayAngle;
        }
        else if (speed < 5.0f) // Walking
        {
            bobFrequency = walkBobFrequency;
            bobHeight = walkBobHeight;
            bobSwayAngle = walkBobSwayAngle;
        }
        else // Running
        {
            bobFrequency = runBobFrequency;
            bobHeight = runBobHeight;
            bobSwayAngle = runBobSwayAngle;
        }

        // Apply the head bob effect if the player is moving
        if (speed > 0.1f)
        {
            timer += Time.deltaTime * bobFrequency;

            // Calculate the new position
            float bobOffset = Mathf.Sin(timer) * bobHeight;
            Vector3 newPosition = startPosition + new Vector3(0, bobOffset, 0);

            // Calculate the new rotation
            float swayOffset = Mathf.Sin(timer) * bobSwayAngle;
            Quaternion newRotation = startRotation * Quaternion.Euler(0, 0, swayOffset);

            // Apply the new position and rotation
            transform.localPosition = newPosition;
            transform.localRotation = newRotation;
        }
        else
        {
            // Reset the position and rotation when idle
            timer = 0.0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * idleBobFrequency);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startRotation, Time.deltaTime * idleBobFrequency);
        }
    }
}
