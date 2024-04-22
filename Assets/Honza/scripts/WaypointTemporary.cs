using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTemporary : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger collision");
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();
        enemyAI.ResetSpeed();
        enemyAI.investigatingWaypoint = false;
        StartCoroutine(enemyAI.ChangePatrolWaypoint());
        Debug.Log("waypoint collision");
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        ClearWaypoint();
    }
    public void ClearWaypoint()
    {
        Destroy(gameObject);
    }
}
