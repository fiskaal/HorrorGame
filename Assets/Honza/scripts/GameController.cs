using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private GameObject waypointPrefab;

    private GameObject currentTempWaypoint;

    public void CreateWaypoint(float speed, int noiseValue, Vector3 waypointPosition, Quaternion waypointRotation)
    {
        Debug.Log("CreateWaypoint called with speed: " + speed + " and noise value of : " + noiseValue);
        GameObject waypoint = Instantiate(waypointPrefab, waypointPosition, waypointRotation);
        WaypointTemporary waypointTemporary = waypoint.GetComponent<WaypointTemporary>();
        waypointTemporary.noiseValue = noiseValue;
        enemyAI.AwarnessMeterUpdate(noiseValue);
        if (!enemyAI.investigatingWaypoint && noiseValue > 0)
        {
            Debug.Log("waypoint created");
            currentTempWaypoint = waypoint;
            enemyAI.InvestigateWaypoint(waypoint.transform, speed);
        }
        else if (enemyAI.investigatingWaypoint && (waypointTemporary.noiseValue >= currentTempWaypoint.GetComponent<WaypointTemporary>().noiseValue))
        {
            Debug.Log("waypoint updated, old one deleted");
            currentTempWaypoint.GetComponent<WaypointTemporary>().ClearWaypoint();
            currentTempWaypoint = waypoint;
            enemyAI.InvestigateWaypoint(waypoint.transform, speed);
        }
        else if (enemyAI.investigatingWaypoint && (waypointTemporary.noiseValue < currentTempWaypoint.GetComponent<WaypointTemporary>().noiseValue))
        {
            Debug.Log("waypoint deleted");
            waypointTemporary.ClearWaypoint();
        }
    }
    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BrokenLeg()
    {
        enemyAI.AwarnessMeterUpdate(100);
    }

}
