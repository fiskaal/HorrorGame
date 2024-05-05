using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockerRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;

    //private LockerController raycastedObj;
    private LockerController lc;

    [SerializeField] private KeyCode openDoorKey = KeyCode.E;

    [SerializeField] private Image crossHair;
    [SerializeField] private GameObject imageE;
    private bool isCrossHairActive;
    private bool doOnce;

    private const string interactableTag = "InteractiveObject";

    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                if (!doOnce)
                {
                    lc = hit.collider.gameObject.GetComponent<LockerController>();
                    CrosshairChange(true);
                }
                isCrossHairActive = true;
                doOnce = true;

                if (Input.GetKeyDown(openDoorKey))
                {
                    lc.PlayAnimation();
                }
            }
        }

        else
        {
            if (isCrossHairActive)
            {
                CrosshairChange(false);
                doOnce = false;
            }
        }
    }

    void CrosshairChange(bool on)
    {
        if (on)
        {
            imageE.SetActive(true);
        }
        else
        {
            imageE.SetActive(false);
        }
    }
}