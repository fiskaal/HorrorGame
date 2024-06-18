using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTemporary : MonoBehaviour
{

    public int noiseValue;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();
        enemyAI.ResetSpeed();
        enemyAI.investigatingWaypoint = false;
        StartCoroutine(enemyAI.ChangePatrolWaypoint());
        Debug.Log("Temp waypoint collision");
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        ClearWaypoint();
    }
    public void ClearWaypoint()
    {
        Debug.Log("destroyed waypoint temp");
        Destroy(gameObject);
    }
}
