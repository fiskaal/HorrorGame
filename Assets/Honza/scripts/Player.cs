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
    private int pickupNoise = -2;
    private int walkingNoise = -2;
    private int runningNoise = -1;

    [SerializeField] private bool playerInSDAS = false;
    [SerializeField] private bool playerInSDAM = false;
    [SerializeField] private bool playerInSDAL = false;
    [SerializeField] private bool isWalkingPrev = false;
    [SerializeField] private bool isRunningPrev = false;

    private void Update()
    {
        if (characterController.isWalking != isWalkingPrev)
        {
            if (characterController.isWalking)
            {
                Debug.Log("character is walking");
                StartCoroutine(PlayerWalking());
                isWalkingPrev = characterController.isWalking;
            }
            else if (!characterController.isWalking)
            {
                Debug.Log("character stopped walking");
                StopAllCoroutines();
                isWalkingPrev = characterController.isWalking;
            }
        }
        if (characterController.isRunning != isRunningPrev)
        {
            if (characterController.isRunning)
            {
                Debug.Log("character is running");
                StartCoroutine(PlayerRunning());
                isRunningPrev = characterController.isRunning;
            }
            else if (!characterController.isRunning)
            {
                Debug.Log("character stopped running");
                StopAllCoroutines();
                isRunningPrev = characterController.isRunning;
            }
        }
    }
    private IEnumerator PlayerWalking()
    {
        Debug.Log("walking playerSDACheck called");
        PlayerInSDACheck(walkingNoise);

        yield return new WaitForSeconds(1.0F);
        StartCoroutine(PlayerWalking());
    }
    private IEnumerator PlayerRunning()
    {
        Debug.Log("running playerSDACheck called");
        PlayerInSDACheck(runningNoise);

        yield return new WaitForSeconds(0.75F);
        StartCoroutine(PlayerRunning());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SDAS"))
        {
            //Debug.Log("player entered SDAS");
            playerInSDAS = true;
        }
        if (other.CompareTag("SDAM"))
        {
            //Debug.Log("player entered SDAM");
            playerInSDAM = true;
        }
        if (other.CompareTag("SDAL"))
        {
            //Debug.Log("player entered SDAL");
            playerInSDAL = true;
        }
        if (other.CompareTag("CDA"))
        {
            Debug.Log("game over triggered");
            gameController.GameOver();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SDAS"))
        {
            //Debug.Log("player left SDAS");
            playerInSDAS = false;
        }
        if (other.CompareTag("SDAM"))
        {
            //Debug.Log("player left SDAM");
            playerInSDAM = false;
        }
        if (other.CompareTag("SDAL"))
        {
            //Debug.Log("player left SDAL");
            playerInSDAL = false;
        }
    }
    public void PlayerInSDACheck(int noiseValue)
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
