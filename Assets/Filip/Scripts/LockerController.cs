using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LockerController : MonoBehaviour
{
    private Animator lockerAnim;

    private bool doorOpen = false;

    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private void Awake()
    {
        lockerAnim = gameObject.GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        if (!doorOpen)
        {
            lockerAnim.Play("LockerOpen", 0, 0.0f);
            doorOpen = true;
            openEvent.Invoke();
        }
        else
        {
            lockerAnim.Play("LockerClose", 0, 0.0f);
            doorOpen = false;
            closeEvent.Invoke();
        }
    }
}
