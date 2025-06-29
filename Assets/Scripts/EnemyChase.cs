
using UnityEngine;
using UnityEngine.AI;
public class EnemyChase : MonoBehaviour
{

    public Transform player;
    public float chaseSpeed = 5f;
    public float wanderSpeed = 2f;
    public float detectionRange = 15f;
    public float wanderInterval = 3f;

    private NavMeshAgent agent;
    private float wanderTimer;

    // Maze bounds
    private float minX = -39f, maxX = 39f;
    private float minZ = -39f, maxZ = 39f;

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



    //public Transform Player;
    //private UnityEngine.AI.NavMeshAgent agent;
    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    //}
    //// Update is called once per frame
    //void Update()
    //{
    //    if (Player != null)
    //    {
    //        agent.SetDestination(Player.position);
    //    }
    //}


