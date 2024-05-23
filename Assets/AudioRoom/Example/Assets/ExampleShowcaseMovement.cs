using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleShowcaseMovement : MonoBehaviour
{
    CharacterController c;
    public float speed,sensitivity;
    public string message;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(message);
        c = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        c.SimpleMove((transform.right*Input.GetAxisRaw("Horizontal")  + transform.forward * Input.GetAxisRaw("Vertical")) * speed*Time.deltaTime);
        transform.Rotate(Vector3.up*Input.GetAxis("Mouse X") *sensitivity *Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Mouse0)) Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;

    }
}
