using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F) && other.CompareTag("Pickup"))
        {
            other.GetComponent<PickUp>().HidePickUp();
            Debug.Log("createWaypoint called");
            gameController.CreateWaypoint();
            if (other.CompareTag("SDAS") || other.CompareTag("SDAM") || other.CompareTag("SDAL"))
            {
                Debug.Log("createWaypoint called");
                gameController.CreateWaypoint();
            }
        }
        /*if ((other.CompareTag("SDAS") || other.CompareTag("SDAM") || other.CompareTag("SDAL")) && *Input.GetKey(KeyCode.F))
        {
            gameController.CreateWaypoint();
        }*/
    }
}
