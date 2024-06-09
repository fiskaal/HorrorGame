using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class WardrobeController : MonoBehaviour
{
    public Animator wardrobeAnim;

    private bool wardrobeOpen = false;

    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private void Awake()
    {
       // wardrobeAnim = gameObject.GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        if (!wardrobeOpen)
        {
            wardrobeAnim.Play("openW", 0, 0.0f);
            //wardrobeAnim.Play("openL", 0, 0.0f);
            wardrobeOpen = true;
            openEvent.Invoke();
        }
        else
        {
            wardrobeAnim.Play("closeW", 0, 0.0f);
            //wardrobeAnim.Play("closeL", 0, 0.0f);
            wardrobeOpen = false;
            closeEvent.Invoke();
        }
    }
}
