using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class SafeController : MonoBehaviour
{
    public Animator safeAnim;

    private bool safeOpen = false;

    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private void Awake()
    {
        //wardrobeAnim = gameObject.GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        if (!safeOpen)
        {
            safeAnim.Play("OpenSafe", 0, 0.0f);
            //wardrobeAnim.Play("openL", 0, 0.0f);
            safeOpen = true;
            openEvent.Invoke();
        }
        else
        {
            safeAnim.Play("CloseSafe", 0, 0.0f);
            //wardrobeAnim.Play("closeL", 0, 0.0f);
            safeOpen = false;
            closeEvent.Invoke();
        }
    }
}
