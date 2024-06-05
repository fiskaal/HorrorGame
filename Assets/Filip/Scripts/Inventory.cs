using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public bool hasRedKey = false;
    public bool hasOrangeKey = false;
    public bool hasBlueKey = false;
    public int totalRocks = 3;

    [SerializeField] private GameObject inventory;
    private bool isOpen = false;

    [SerializeField] private GameObject redKey;
    [SerializeField] private GameObject orangeKey;
    [SerializeField] private GameObject blueKey;
    [SerializeField] private GameObject[] rocks = new GameObject[3];
    private int rocksArrayIndex = 2;

    public void Update()
    {
        if (!isOpen && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.B)))
        {
            inventory.SetActive(true);
            isOpen = true;
        }
        else if (isOpen && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape)))
        {
            inventory.SetActive(false);
            isOpen = false;
        }
    }
    public void SubstractRock()
    {
        rocks[rocksArrayIndex].gameObject.SetActive(false);
        rocksArrayIndex--;
        totalRocks--;
    }
    public void AddtRock()
    {
        totalRocks++;
        rocksArrayIndex++;
        rocks[rocksArrayIndex].gameObject.SetActive(true);
    }
    public void PickedUpRedKey()
    {
        hasRedKey = true;
        redKey.SetActive(true);
    }
    public void PickedUpOrangeKey()
    {
        hasOrangeKey = true;
        orangeKey.SetActive(true);
    }
    public void PickedUpBlueKey()
    {
        hasBlueKey = true;
        blueKey.SetActive(true);
    }
}
