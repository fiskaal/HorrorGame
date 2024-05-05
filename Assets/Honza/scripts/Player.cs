using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private ExamplePlayerController characterController;
    private float speedSDAS = 8F;
    private float speedSDAM = 6F;
    private float speedSDAL = 4.5F;
    private int pickupNoise = 1;
    private int walkingNoise = 1;
    private int runningNoise = 2;
    [SerializeField] private bool playerInSDAS = false;
    [SerializeField] private bool playerInSDAM = false;
    [SerializeField] private bool playerInSDAL = false;
    private bool isWalkingPrev = false;
    private bool isRunningPrev = false;

    private void Update()
    {
        if (characterController.isWalking != isWalkingPrev)
        {
            StopCoroutine(PlayerWalking());
            StartCoroutine(PlayerWalking());
            isWalkingPrev = characterController.isWalking;
        }
        if (characterController.isRunning != isRunningPrev)
        {
            StopCoroutine(PlayerWalking());
            StartCoroutine(PlayerRunning());
            isRunningPrev = characterController.isRunning;
        }
    }
    private IEnumerator PlayerWalking()
    {
        PlayerInSDACheck(walkingNoise);

        yield return new WaitForSeconds(1.0F);
    }
    private IEnumerator PlayerRunning()
    {
        PlayerInSDACheck(runningNoise);

        yield return new WaitForSeconds(0.75F);
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F) && other.CompareTag("Pickup"))
        {
            other.GetComponent<PickUp>().HidePickUp();
            PlayerInSDACheck(pickupNoise);
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
    private void PlayerInSDACheck(int noiseValue)
    {
        Debug.Log("PlayerInSDACheck called");
        if (playerInSDAS)
        {
            gameController.CreateWaypoint(speedSDAS, noiseValue + 3);
        }
        else if (playerInSDAM)
        {
            gameController.CreateWaypoint(speedSDAM, noiseValue + 2);
        }
        else if (playerInSDAL)
        {
            gameController.CreateWaypoint(speedSDAL, noiseValue + 1);
        }
    }
}
