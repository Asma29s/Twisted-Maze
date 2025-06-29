using UnityEngine;
using UnityEngine.AI;

public class ChaseTesting : MonoBehaviour
{
    public Transform Player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Player != null)
        {
            agent.SetDestination(Player.position);
        }
    }
}
