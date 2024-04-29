using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{

    [SerializeField] private bool _enabled = true;
    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 20f)] private float frequency = 10f;

    [SerializeField] private Transform camera;
    [SerializeField] private Transform cameraHolder;

    private float toggleSpeed = 3.0f;
    private Vector3 startPos;
    public CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        startPos = camera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enabled) return;

        CheckMotion();
        ResetPosition();
        
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.x += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.y += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        if (speed < toggleSpeed) return;

        if (!controller.isGrounded) return;

        //PlayMotion(FootStepMotion);
    }

    private void ResetPosition()
    {
        if (camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 * Time.deltaTime);
    }
}
