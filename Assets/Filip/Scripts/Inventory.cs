using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public bool hasRedKey = false;
    public bool hasOrangeKey = false;
    public bool hasBlueKey = false;
    public int totalRocks = 10;

    [SerializeField] private GameObject inventoryUI;
    private bool isOpen = false;

    [SerializeField] private GameObject RedKey;
    [SerializeField] private GameObject OrangeKey;
    [SerializeField] private GameObject BlueKey;
    [SerializeField] private GameObject Rocks;

    public void Update()
    {
        if (!isOpen && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.B)))
        {
            inventoryUI.SetActive(true);
            isOpen = true;
        }
        else if (isOpen && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape)))
        {
            inventoryUI.SetActive(false);
            isOpen = false;
        }
    }

    private void ShowItem(GameObject item)
    {
        item.SetActive(true);
    }
    private void HideItem(GameObject item)
    {
        item.SetActive(false);
    }
}
