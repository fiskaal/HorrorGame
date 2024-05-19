using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform[] patrolWaypoints = new Transform[4];
    [SerializeField] private Transform playerTransform;
    private int lastPatrolWaypoint = -1;
    public bool investigatingWaypoint = false;
    private bool chasingPlayer = false;
    [SerializeField] private int AwarnessMeter = 0;

    Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.3F;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        StartPatroling();
        StartCoroutine(AwarnessMeterDecay());
        
    }
    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, smoothTime);
        if (chasingPlayer)
            ChasePlayer();
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
    private IEnumerator AwarnessMeterDecay()
    {
        if (AwarnessMeter > 0)
            AwarnessMeter--;

        yield return new WaitForSeconds(1.0f);

        StartCoroutine(AwarnessMeterDecay());
    }
    public void AwarnessMeterUpdate(int value)
    {
        AwarnessMeter += value;
        if (AwarnessMeter >= 10)
        {
            StopAllCoroutines();
            chasingPlayer = true;
            agent.speed = 8F;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
    }
}
