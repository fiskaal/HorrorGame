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
                StartCoroutine(PlayerWalking());
                isWalkingPrev = characterController.isWalking;
            }
            else if (!characterController.isWalking)
            {
                StopAllCoroutines();
                isWalkingPrev = characterController.isWalking;
            }
        }
        if (characterController.isRunning != isRunningPrev)
        {
            if (characterController.isRunning)
            {
                StartCoroutine(PlayerRunning());
                isRunningPrev = characterController.isRunning;
            }
            else if (!characterController.isRunning)
            {
                StopAllCoroutines();
                isRunningPrev = characterController.isRunning;
            }
        }
    }
    private IEnumerator PlayerWalking()
    {
        PlayerInSDACheck(walkingNoise);

        yield return new WaitForSeconds(1.0F);
        StartCoroutine(PlayerWalking());
    }
    private IEnumerator PlayerRunning()
    {
        PlayerInSDACheck(runningNoise);

        yield return new WaitForSeconds(0.75F);
        StartCoroutine(PlayerRunning());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SDAS"))
            playerInSDAS = true;
        if (other.CompareTag("SDAM"))
            playerInSDAM = true;
        if (other.CompareTag("SDAL"))
            playerInSDAL = true;
        if (other.CompareTag("CDA"))
            gameController.GameOver();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SDAS"))
            playerInSDAS = false;
        if (other.CompareTag("SDAM"))
            playerInSDAM = false;
        if (other.CompareTag("SDAL"))
            playerInSDAL = false;
    }
    public void PlayerInSDACheck(int noiseValue)
    {
        if (playerInSDAS)
        {
            gameController.CreateWaypoint(speedSDAS, noiseValue + 3, transform.position, transform.rotation);
        }
        else if (playerInSDAM)
        {
            gameController.CreateWaypoint(speedSDAM, noiseValue + 2, transform.position, transform.rotation);
        }
        else if (playerInSDAL)
        {
            gameController.CreateWaypoint(speedSDAL, noiseValue + 1, transform.position, transform.rotation);
        }
    }
}
