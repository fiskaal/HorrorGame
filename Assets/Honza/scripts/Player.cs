using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    private float speedSDAS = 8F;
    private float speedSDAM = 6F;
    private float speedSDAL = 4.5F;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F) && other.CompareTag("Pickup"))
        {
            other.GetComponent<PickUp>().HidePickUp();
            //Debug.Log("createWaypoint called");
            //gameController.CreateWaypoint();
            if (other.CompareTag("SDAS"))
            {
                Debug.Log("createWaypoint SDAS called");
                gameController.CreateWaypoint(speedSDAS);
            }
            else if (other.CompareTag("SDAM"))
            {
                Debug.Log("createWaypoint SDAM called");
                gameController.CreateWaypoint(speedSDAM);
            }
            else if (other.CompareTag("SDAL"))
            {
                Debug.Log("createWaypoint SDAL called");
                gameController.CreateWaypoint(speedSDAL);
            }
        }
        /*if ((other.CompareTag("SDAS") || other.CompareTag("SDAM") || other.CompareTag("SDAL")) && *Input.GetKey(KeyCode.F))
        {
            gameController.CreateWaypoint();
        }*/
    }
}
