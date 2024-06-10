using UnityEngine;
using System.Collections;

public class ExamplePlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameController gameController;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float mouseSensitivity = 50f;
    [SerializeField] private float fallThresholdVelocity = 7.5f;

    private float originalHeight;
    private Vector3 velocity;
    private bool isCrouching = false;
    private float xRotation = 0f;

    public bool isWalking = false;
    public bool isRunning = false;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool brokenLeg = false;

    /*
    [Header("Headbob stats")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.5f;

    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 1f;

    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.25f;

    private float defaultYpos;
    private float timer;
    */

    void Start()
    {
        originalHeight = controller.height;
        //Cursor.lockState = CursorLockMode.Locked;
        //defaultYpos = playerCamera.transform.localPosition.y;
    }

    void Update()
    {
        bool wasGrounded = isGrounded;
        //GroundCheck
        Vector3 capsuleBottom = transform.position + controller.center - Vector3.up * (controller.height / 2);
        Vector3 capsuleTop = transform.position + controller.center + Vector3.up * (controller.height / 2);
        isGrounded = Physics.CheckCapsule(capsuleBottom, capsuleTop, controller.radius + groundCheckDistance, groundMask);

        if (!wasGrounded && isGrounded)
        {
            //Debug.Log("Do damage: " + (controller.velocity.y < -fallThresholdVelocity));
            if (controller.velocity.y < -fallThresholdVelocity)
            {
                BrokenLeg();
            }
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensure the character stays grounded
            
        }

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1 && !brokenLeg)
            move = move.normalized;

        if (Input.GetKey(KeyCode.LeftShift) && !brokenLeg)
        {
            move *= runSpeed;
            isRunning = true;
            isWalking = false;
        }
        else if (isCrouching && !brokenLeg)
        {
            move *= crouchSpeed;
            isWalking = false;
            isRunning = false;
        }
        else if (!brokenLeg)
        {
            move *= speed;
            if (move.magnitude > 1)
            {
                isWalking = true;
                isRunning = false;
            }
            else
            {
                isWalking = false;
                isRunning = false;
            }
        }

        if (!brokenLeg)
            controller.Move(move * Time.deltaTime);

        // Jumping
        if (Input.GetKey(KeyCode.Space) && Physics.Raycast(transform.TransformPoint(new Vector3(0, -.9f, 0)), Vector3.down, .2f) && !brokenLeg)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl) && !brokenLeg)
        {
            isCrouching = !isCrouching;
            Vector3 change = transform.localScale;
            change.y *= isCrouching ? .5f : 2f;
            transform.localScale = change;
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    private void BrokenLeg()
    {
        brokenLeg = true;
        gameController.CreateWaypoint(8, 10, transform.position, transform.rotation);
    }
    public void HandleHeadbob()
    {

    }
}
