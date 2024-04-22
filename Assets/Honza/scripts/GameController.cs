using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private GameObject waypointPrefab;

    [SerializeField] private SphereCollider soundDetectAreaSmall;
    [SerializeField] private SphereCollider soundDetectAreaMedium;
    [SerializeField] private SphereCollider soundDetectAreaLarge;
    [SerializeField] private CapsuleCollider playerCollider;

    private GameObject currentTempWaypoint;

    public void CreateWaypoint(float speed)
    {
        Debug.Log("CreateWaypoint called with speed: " + speed);
        GameObject waypoint = Instantiate(waypointPrefab, player.transform.position, player.transform.rotation);
        if (enemyAI.investigatingWaypoint)
        {
            WaypointTemporary waypointTemporary = currentTempWaypoint.GetComponent<WaypointTemporary>();
            waypointTemporary.ClearWaypoint();
        }
        currentTempWaypoint = waypoint;
        enemyAI.InvestigateWaypoint(waypoint.transform, speed);
    }

}
