using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class FuseBoxController : MonoBehaviour
{
    public Animator fuseboxAnim;

    private bool fuseboxOpen = false;

    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private void Awake()
    {
        //wardrobeAnim = gameObject.GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        if (!fuseboxOpen)
        {
            fuseboxAnim.Play("OpenFuseBox", 0, 0.0f);
            //wardrobeAnim.Play("openL", 0, 0.0f);
            fuseboxOpen = true;
            openEvent.Invoke();
        }
        else
        {
            fuseboxAnim.Play("CloseFuseBox", 0, 0.0f);
            //wardrobeAnim.Play("closeL", 0, 0.0f);
            fuseboxOpen = false;
            closeEvent.Invoke();
        }
    }
}
