using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DrawerController : MonoBehaviour
{
    private Animator drawerAnim;

    private bool drawerOpen = false;

    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private void Awake()
    {
        drawerAnim = gameObject.GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        if (!drawerOpen)
        {
            drawerAnim.Play("openDrawer", 0, 0.0f);
            drawerOpen = true;
            openEvent.Invoke();
        }
        else
        {
            drawerAnim.Play("closeDrawer", 0, 0.0f);
            drawerOpen = false;
            closeEvent.Invoke();
        }
    }
}
