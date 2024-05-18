using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private float speedSDAS = 8F;
    private float speedSDAM = 6F;
    private float speedSDAL = 4.5F;
    private int rockImpactNoise = 2;

    [SerializeField] private bool rockInSDAS = false;
    [SerializeField] private bool rockInSDAM = false;
    [SerializeField] private bool rockInSDAL = false;

    private void OnCollisionEnter(Collision collision)
    {
        gameController = FindObjectOfType<GameController>();
        RockInSDACheck(rockImpactNoise);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SDAS"))
        {
            //Debug.Log("player entered SDAS");
            rockInSDAS = true;
        }
        if (other.CompareTag("SDAM"))
        {
            //Debug.Log("player entered SDAM");
            rockInSDAM = true;
        }
        if (other.CompareTag("SDAL"))
        {
            //Debug.Log("player entered SDAL");
            rockInSDAL = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SDAS"))
        {
            //Debug.Log("player left SDAS");
            rockInSDAS = false;
        }
        if (other.CompareTag("SDAM"))
        {
            //Debug.Log("player left SDAM");
            rockInSDAM = false;
        }
        if (other.CompareTag("SDAL"))
        {
            //Debug.Log("player left SDAL");
            rockInSDAL = false;
        }
    }
    public void RockInSDACheck(int noiseValue)
    {
        Debug.Log("RockInSDACheck called");
        if (rockInSDAS)
        {
            gameController.CreateWaypoint(speedSDAS, noiseValue + 3, transform.position, transform.rotation);
        }
        else if (rockInSDAM)
        {
            gameController.CreateWaypoint(speedSDAM, noiseValue + 2, transform.position, transform.rotation);
        }
        else if (rockInSDAL)
        {
            gameController.CreateWaypoint(speedSDAL, noiseValue + 1, transform.position, transform.rotation);
        }
    }
}
