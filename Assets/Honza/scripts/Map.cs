using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField] private KeyCode openMapKey = KeyCode.M;
    [SerializeField] private GameObject mapUI;

    [SerializeField] private GameObject lobbyZoneMarker;

    private bool isMapOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMapOpen = !isMapOpen;

            ToggleMap(isMapOpen);
        }
    }

    void ToggleMap(bool open)
    {
        if (open)
            mapUI.SetActive(true);
        else
            mapUI.SetActive(false);
    }

    public void MarkZoneAsLooted(GameObject zone)
    {
        zone.SetActive(true);
    }
}
