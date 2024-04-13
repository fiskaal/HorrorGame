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
        StartCoroutine(enemyAI.ChangePatrolWaypoint());
        Debug.Log("waypoint collision");
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        Destroy(gameObject);
    }
}
