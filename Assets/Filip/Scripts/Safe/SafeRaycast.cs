using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeRaycast : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameController gameController;
    [SerializeField] private int rayLength = 2;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;

    //private LockerController raycastedObj;
    private SafeController sc;
    private int openDoorNoise = -1;

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
                    sc = hit.collider.gameObject.GetComponent<SafeController>();
                    CrosshairChange(true);
                }
                isCrossHairActive = true;
                doOnce = true;

                if (Input.GetKeyDown(openDoorKey))
                {
                    //player.PlayerInSDACheck(openDoorNoise);
                    sc.PlayAnimation();
                    player.PlayerInSDACheck(openDoorNoise);
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
