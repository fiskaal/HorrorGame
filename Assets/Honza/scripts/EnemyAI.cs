using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform[] patrolWaypoints = new Transform[4];
    private int lastPatrolWaypoint = -1;
    public bool investigatingWaypoint = false;

    Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.3F;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        StartPatroling();
    }
    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, smoothTime);
    }

    void StartPatroling()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, patrolWaypoints.Length);
            Debug.Log("patroling loop with index: " + randomIndex);
        } while (randomIndex.Equals(lastPatrolWaypoint));

        agent.SetDestination(patrolWaypoints[randomIndex].position);
        lastPatrolWaypoint = randomIndex;
    }
    public IEnumerator ChangePatrolWaypoint()
    {
        Debug.Log("ChangePatrolWaypoint called");
        yield return new WaitForSeconds(5.0f);

        StartPatroling();
        Debug.Log("StartPatroling called");
    }
    public void InvestigateWaypoint(Transform waypoint, float speed)
    {
        Debug.Log("InvestigateWaypoint called");
        agent.isStopped.Equals("true");
        agent.speed = speed;
        agent.SetDestination(waypoint.position);
        investigatingWaypoint = true;
    }
    public void ResetSpeed()
    {
        agent.speed = 4.5F;
    }
}
