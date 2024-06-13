using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    private Animator doorAnim;

    private bool doorOpen = false;

    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        if (!doorOpen)
        {
            doorAnim.Play("Open", 0, 0.0f);
            doorOpen = true;
            openEvent.Invoke();
        }
        else
        {
            doorAnim.Play("Closed", 0, 0.0f);
            doorOpen = false;
            closeEvent.Invoke();
        }
    }
}