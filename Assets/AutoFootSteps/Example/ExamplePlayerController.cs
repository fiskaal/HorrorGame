using UnityEngine;

public class ExamplePlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5.0f;
    public float runSpeed = 10.0f;
    public float crouchSpeed = 2.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    public Camera playerCamera;
    public float crouchHeight = 0.5f;
    public float mouseSensitivity = 100f;

    private float originalHeight;
    private Vector3 velocity;
    private bool isCrouching = false;
    private float xRotation = 0f;

    void Start()
    {
        originalHeight = controller.height;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1)
            move = move.normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            move *= runSpeed;
        }
        else if (isCrouching)
        {
            move *= crouchSpeed;
        }
        else
        {
            move *= speed;
        }

        controller.Move(move * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && Physics.Raycast(transform.TransformPoint(new Vector3(0, -.9f, 0)), Vector3.down, .2f))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
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
}
