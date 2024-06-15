using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();

        if (!other.gameObject.CompareTag("Enemy") || enemyAI.investigatingWaypoint)
            return;

        StartCoroutine(enemyAI.ChangePatrolWaypoint());
        Debug.Log("waypoint collision");
    }
}
