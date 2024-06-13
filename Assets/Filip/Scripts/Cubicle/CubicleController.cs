using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CubicleController : MonoBehaviour
{
    private Animator cubicleAnim;

    private bool doorOpen = false;

    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private void Awake()
    {
        cubicleAnim = gameObject.GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        if (!doorOpen)
        {
            cubicleAnim.Play("CubicleDoorOpen", 0, 0.0f);
            doorOpen = true;
            openEvent.Invoke();
        }
        else
        {
            cubicleAnim.Play("CubicleDoorClose", 0, 0.0f);
            doorOpen = false;
            closeEvent.Invoke();
        }
    }
}
