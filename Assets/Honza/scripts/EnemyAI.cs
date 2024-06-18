using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Animator jefAnimator;
    [SerializeField] private Transform[] patrolWaypoints = new Transform[4];
    [SerializeField] private Transform playerTransform;
    [SerializeField] private UnityEvent walkingEvent;
    [SerializeField] private UnityEvent growlEvent1;
    [SerializeField] private UnityEvent growlEvent2;
    [SerializeField] private UnityEvent growlEvent3;
    [SerializeField] private AudioSource footstepsAudio;
    [SerializeField] private UnityEvent HuntingEventStart;
    [SerializeField] private UnityEvent HuntingEventStop;
    private int lastPatrolWaypoint = -1;
    public bool investigatingWaypoint = false;
    private bool chasingPlayer = false;
    [SerializeField] private int AwarnessMeter = 0;

    Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.3F;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.updatePosition = false;
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
        footstepsAudio.mute = false;
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, patrolWaypoints.Length);
        } while (randomIndex.Equals(lastPatrolWaypoint));
        jefAnimator.Play("Walk", 0, 0.0f);
        agent.SetDestination(patrolWaypoints[randomIndex].position);
        lastPatrolWaypoint = randomIndex;
    }
    public IEnumerator ChangePatrolWaypoint()
    {
        jefAnimator.Play("Attentione", 0, 0.0f);
        footstepsAudio.mute = true;
        yield return new WaitForSeconds(1.0f);
        growlEvent1.Invoke();
        yield return new WaitForSeconds(3.0f);
        growlEvent2.Invoke();
        yield return new WaitForSeconds(2.0f);
        growlEvent3.Invoke();
        yield return new WaitForSeconds(2.0f);

        StartPatroling();
    }
    public void InvestigateWaypoint(Transform waypoint, float speed)
    {
        jefAnimator.Play("Walk", 0, 0.0f);
        Debug.Log("InvestigateWaypoint called");
        agent.isStopped.Equals("true");
        agent.speed = speed;
        agent.SetDestination(waypoint.position);
        investigatingWaypoint = true;
    }
    public void ResetSpeed()
    {
        agent.speed = 2.5F;
    }
    private IEnumerator AwarnessMeterDecay()
    {
        //Debug.Log("AwarnessMeterDecay called");
        if (AwarnessMeter > 0)
            AwarnessMeter--;
        if (AwarnessMeter < 10 && chasingPlayer)
        {
            chasingPlayer = false;
            HuntingEventStop.Invoke();
            //StopAllCoroutines();
            ResetSpeed();
            StartCoroutine(ChangePatrolWaypoint());
        }

        yield return new WaitForSeconds(2.0f);

        StartCoroutine(AwarnessMeterDecay());
    }
    public void AwarnessMeterUpdate(int value)
    {
        AwarnessMeter += value;
        if (AwarnessMeter >= 10 && !chasingPlayer)
        {
            investigatingWaypoint = false;
            //StopAllCoroutines();
            chasingPlayer = true;
            HuntingEventStart.Invoke();
            agent.speed = 4.5F;
        }
    }
    private void ChasePlayer()
    {
        jefAnimator.Play("Walk", 0, 0.0f);
        agent.SetDestination(playerTransform.position);
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DoorClosedCollider"))
        {
            ResetSpeed();
            investigatingWaypoint = false;
            StartCoroutine(ChangePatrolWaypoint());
            Debug.Log("door collision");
        }
    }*/
    /*public void DoorCollision()
    {
        ResetSpeed();
        investigatingWaypoint = false;
        StartCoroutine(ChangePatrolWaypoint());
        Debug.Log("door collision");
    }*/
}
