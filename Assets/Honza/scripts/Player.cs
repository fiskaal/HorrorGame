using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    private float speedSDAS = 8F;
    private float speedSDAM = 6F;
    private float speedSDAL = 4.5F;
    [SerializeField] private bool playerInSDAS = false;
    [SerializeField] private bool playerInSDAM = false;
    [SerializeField] private bool playerInSDAL = false;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F) && other.CompareTag("Pickup"))
        {
            other.GetComponent<PickUp>().HidePickUp();
            PlayerInSDACheck();
        }
        if (Input.GetKeyDown(KeyCode.F) && other.CompareTag("HidingSpot"))
        {
            other.GetComponent<HidingSpot>().SwitchView();
            PlayerInSDACheck();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SDAS"))
        {
            Debug.Log("player entered SDAS");
            playerInSDAS = true;
        }
        if (other.CompareTag("SDAM"))
        {
            Debug.Log("player entered SDAM");
            playerInSDAM = true;
        }
        if (other.CompareTag("SDAL"))
        {
            Debug.Log("player entered SDAL");
            playerInSDAL = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SDAS"))
        {
            Debug.Log("player left SDAS");
            playerInSDAS = false;
        }
        if (other.CompareTag("SDAM"))
        {
            Debug.Log("player left SDAM");
            playerInSDAM = false;
        }
        if (other.CompareTag("SDAL"))
        {
            Debug.Log("player left SDAL");
            playerInSDAL = false;
        }
    }
    private void PlayerInSDACheck()
    {
        Debug.Log("PlayerInSDACheck called");
        if (playerInSDAS)
        {
            gameController.CreateWaypoint(speedSDAS);
        }
        else if (playerInSDAM)
        {
            gameController.CreateWaypoint(speedSDAM);
        }
        else if (playerInSDAL)
        {
            gameController.CreateWaypoint(speedSDAL);
        }
    }
}
