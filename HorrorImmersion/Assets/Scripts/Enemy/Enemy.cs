using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    /* 
     * INTENDED BEHAVIOUR
     * - Respond to sounds made by the player and search the origin of the sound (depending on sound level)
     * - Chase player when they are in `visible` range
     *      - `visible` range should change depending on the light level
     * - Stop chasing player when line of sight is broken twice in a row (similar to Phasmophobia)
     * - Patrol around the house when not chasing the player
     */
    [Header("General Attributes")]
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform enemyHeadPosition;
    [SerializeField] Transform player;
    [SerializeField] private float enemy_interactRange;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Raycast Attributes")]
    [SerializeField] private int rayCount;
    [SerializeField] private float coneAngle;
    [SerializeField] private float visibleRange;

    [Header("Hearing Ability")]
    [SerializeField] private EnemyReactToSound enemyHearing;

    private bool isPatrolling = false;
    private bool hasPatrolledThisFrame = false;

    private void Update()
    {
        hasPatrolledThisFrame = false;
        ConeShapeRayBehaviour();
        RotateTowardsDestination();
        ReactToSound();
    }

    /// <summary>
    /// Extend `rayCount` amount of rays originating from current position to visibleRange
    /// in a cone-like pattern
    /// </summary>
    private void ConeShapeRayBehaviour()
    {
        float angleBtwnRays = coneAngle / (rayCount - 1);

        for(int i = 0; i < rayCount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, -coneAngle / 2 + i * angleBtwnRays, 0);
            Vector3 rayDirection = rotation * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, visibleRange))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                if (hit.collider.CompareTag("Player") == true)
                {
                    ChasePlayer();
                }
                // Give enemy the ability to open doors when patroling
                if (isPatrolling)
                {
                    if(hit.collider.CompareTag("Door") == true)
                    {
                        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                        Doors door = hit.collider.GetComponent<Doors>();

                        if(interactable != null && door.isDoorOpen() == false && Vector3.Distance(transform.position, hit.transform.position) <= enemy_interactRange)
                        {
                            interactable.Interact();
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position, rayDirection * visibleRange, Color.green);

                if (!isPatrolling && !hasPatrolledThisFrame)
                {
                    //Debug.Log("isPatrolling: " + isPatrolling);
                    Patrol();
                    hasPatrolledThisFrame = true;
                }
                else
                {
                    if (enemy.remainingDistance <= enemy.stoppingDistance)
                    {
                        isPatrolling = false;
                    }
                }
            }
        }
    }

    private void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }

    private void Patrol()
    {
        isPatrolling = true;
        //Debug.Log("isPatrolling: " + isPatrolling);
        
        int rand = Random.Range(0, patrolPoints.Length);

        enemy.SetDestination(patrolPoints[rand].position);
        //Debug.Log("Destination: " + patrolPoints[rand].position);
    }

    void RotateTowardsDestination()
    {
        Vector3 direction = enemy.steeringTarget - transform.position;
        direction.y = 0;
        if (enemy.velocity.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * enemy.angularSpeed);
        }
    }

    private void ReactToSound()
    {
        if (enemyHearing.GetSoundHeard())
        {
            Debug.Log("Enemy Checking Sound");
            enemy.SetDestination(enemyHearing.GetLocationSoundWasHeard());
        }
    }
}
