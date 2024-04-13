using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent agent;
    [SerializeField] private Transform[] patrolWaypoints = new Transform[4];
    //private Transform followTarget;
    [SerializeField] private SphereCollider soundDetectAreaSmall;
    [SerializeField] private SphereCollider soundDetectAreaMedium;
    [SerializeField] private SphereCollider soundDetectAreaLarge;
    private int lastPatrolWaypoint = -1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartPatroling();
    }

    void StartPatroling()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, patrolWaypoints.Length);
            Debug.Log("patroling loop with index: " + randomIndex);
        } while (randomIndex.Equals(lastPatrolWaypoint));

        agent.destination = patrolWaypoints[randomIndex].position;
        lastPatrolWaypoint = randomIndex;

    }
    public IEnumerator ChangePatrolWaypoint()
    {
        Debug.Log("ChangePatrolWaypoint called");
        yield return new WaitForSeconds(5.0f);

        StartPatroling();
        Debug.Log("StartPatroling called");
    }
    public void InvestigateWaypoint(Transform waypoint)
    {
        Debug.Log("InvestigateWaypoint called");
        agent.isStopped.Equals("true");
        agent.destination = waypoint.position;
    }
    void ChasePlayer()
    {
        agent.destination = player.position;
    }
}
