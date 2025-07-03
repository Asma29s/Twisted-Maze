using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{

    public Animator animator; 
    public float jumpScareDuration = 3f;

    public void TriggerJumpScare()
    {
        animator.SetTrigger("JumpScare"); // this is the parameter name in the animation controller settings 
    }

    public float GetJumpScareDuration()
    {
        return jumpScareDuration;
    }


    [Header("References")]
    public Transform player;

    [Header("Speeds")]
    public float chaseSpeed = 5f;
    public float wanderSpeed = 2f;

    [Header("Detection")]
    public float detectionRange = 15f;

    [Header("Wandering")]
    public float wanderInterval = 15f;
    private float wanderTimer;

    [Header("Maze Bounds")]
    private float minX = -154f, maxX = 154f;
    private float minZ = -154f, maxZ = 154f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderInterval;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && HasLineOfSight())
        {
            // Player in sight: chase
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            // Player not in sight: wander
            agent.speed = wanderSpeed;
            WanderRandomly();

            // If agent reached destination, immediately get a new one
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    WanderRandomly(); // Get new random destination immediately
                }
            }
        }
    }

    void WanderRandomly()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderInterval)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(minX, maxX),
                0,
                Random.Range(minZ, maxZ)
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            wanderTimer = 0;
        }
    }

    bool HasLineOfSight()
    {
        RaycastHit hit;
        Vector3 direction = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position + Vector3.up, direction, out hit, detectionRange))
        {
            return hit.transform == player;
        }

        return false;
    }
}
