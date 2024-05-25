using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField] private KeyCode openMapKey = KeyCode.M;
    [SerializeField] private GameObject mapUI;

    [SerializeField] private GameObject lobbyZoneMarker;
    public bool note1Opened = false;
    public bool note2Opened = false;
    public bool note3Opened = false;

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

    public void LootedZoneCheck()
    {
        if (note1Opened && note2Opened)
            MarkZoneAsLooted(lobbyZoneMarker);

    }

    public void MarkZoneAsLooted(GameObject zone)
    {
        zone.SetActive(true);
    }
}
