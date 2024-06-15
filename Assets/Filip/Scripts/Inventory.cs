using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public bool hasRedKey = false;
    public bool hasOrangeKey = false;
    public bool hasBlueKey = false;
    public bool hasSafeKey = false;
    public int totalRocks = 3;

    [SerializeField] private GameObject inventory;
    private bool isOpen = false;

    [SerializeField] private GameObject redKey;
    [SerializeField] private GameObject orangeKey;
    [SerializeField] private GameObject blueKey;
    [SerializeField] private GameObject safeKey;
    [SerializeField] private GameObject[] rockArray = new GameObject[3];
    private int rocksArrayIndex = 2;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private PauseMenu pauseMenu;

    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    [SerializeField] private LayerMask inventoryItemLayer; // Layer mask for the inventory items
    [SerializeField] private Transform itemHoldingPoint;
    [SerializeField] private Transform ThrowingPoint;
    [SerializeField] private RockThrowing rockThrowing;
    [SerializeField] private GameObject activeItem;
    [SerializeField] private GameObject lastActiveItem;

    public void Update()
    {
        if (!isOpen && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.B)))
        {
            StartCoroutine(WaitForUI(.5f));
            Debug.Log("inventory open");
        }
        else if (isOpen && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape)))
        {
            ContinueGame();
            Debug.Log("inventory closed");
        }

        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            // Cast a ray from the cursor position into the scene
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check for intersections with the inventory items
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, inventoryItemLayer))
            {
                // Handle the click event for the inventory item
                GameObject clickedObject = hit.collider.gameObject;
                HandleItemClick(clickedObject);
            }
        }
    }
    void HandleItemClick(GameObject item)
    {
        // Implement your logic here to handle the click event for the inventory item
        Debug.Log("Clicked on: " + item.name);
        // Example: Select the item, trigger an action, etc.
        if (activeItem != null)
        {
            lastActiveItem.SetActive(true);
            Destroy(activeItem);
        }
        if (item.tag.Equals("Rock"))
        {
            activeItem = Instantiate(
            item,
            ThrowingPoint.position,
            ThrowingPoint.rotation);
            activeItem.transform.SetParent(ThrowingPoint);
            rockThrowing.ReadyThrow(activeItem);
        }
        else
        {
            activeItem = Instantiate(
            item,
            itemHoldingPoint.position,
            itemHoldingPoint.rotation);
            activeItem.transform.SetParent(itemHoldingPoint);
        }
        lastActiveItem = item;
        item.SetActive(false);
    }
    public void SubstractRock()
    {
        rocksArrayIndex--;
        totalRocks--;
    }
    public void AddtRock()
    {
        if (totalRocks < 3)
        {
            for (int i = 0; i < totalRocks; i++)
            {
                if (!rockArray[i].active)
                {
                    rockArray[i].SetActive(true);
                    totalRocks++;
                    rocksArrayIndex++;
                    break;
                }
            }
        }
    }
    public void ActiveItemUsed()
    {
        activeItem = null;
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

    public void PickedUpSafeKey()
    {
        hasSafeKey = true;
        safeKey.SetActive(true);
    }
    public void PauseGame()
    {
        inventory.SetActive(true);
        crossHair.SetActive(false);
        isOpen = true;
        pauseMenu.inventoryOpened = true;
        Time.timeScale = 0.1F;
        //imageAnimator.Play("Image50FadeIn");

    }
    public void ContinueGame()
    {
        inventory.SetActive(false);
        crossHair.SetActive(true);
        isOpen = false;
        pauseMenu.inventoryOpened = false;
        Time.timeScale = 1;
    }

    IEnumerator WaitAndPlay(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1;
        PauseGame();
        ContinueGame();

    }

    IEnumerator WaitForUI(float waitTime)
    {
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = 0;
        PauseGame();


    }
}
