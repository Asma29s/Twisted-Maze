using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class WallPhaseManager : MonoBehaviour
{
    public List<GameObject> wallPhases;   // Wall prefabs for each phase
    private int currentPhase = 0;

    public NavMeshSurface navMeshSurface; // Assign this from the Inspector

    private void Start()
    {
        ActivatePhase(currentPhase);
        BuildNavMesh();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchToNextPhase();
        }
    }

    void ActivatePhase(int index)
    {
        for (int i = 0; i < wallPhases.Count; i++)
        {
            wallPhases[i].SetActive(i == index);
        }
    }

    void SwitchToNextPhase()
    {
        // Switch wall
        wallPhases[currentPhase].SetActive(false);
        currentPhase = (currentPhase + 1) % wallPhases.Count;
        wallPhases[currentPhase].SetActive(true);

        // Trigger NavMesh update
        BuildNavMesh();
    }

    void BuildNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.RemoveData();   // Clear previous mesh (prevents stacking)
            navMeshSurface.BuildNavMesh(); // Rebuild (you can use UpdateNavMesh() for future tile-based setups)
        }
    }
}
