using UnityEngine;
using UnityEngine.AI;


public class EnemyChase : MonoBehaviour
{

    public Transform Player;
    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

}

// Update is called once per frame
void Update()
    {
        if (Player != null)
        {
            agent.SetDestination(Player.position);
        }

    }
}
