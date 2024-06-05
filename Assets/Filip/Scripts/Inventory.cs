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
    [SerializeField] private GameObject crossHair;

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
    public void PauseGame()
    {
        inventory.SetActive(true);
        crossHair.SetActive(false);
        isOpen = true;
        Time.timeScale = 0.1F;
        //imageAnimator.Play("Image50FadeIn");

    }
    public void ContinueGame()
    {
        inventory.SetActive(false);
        crossHair.SetActive(true);
        isOpen = false;
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
