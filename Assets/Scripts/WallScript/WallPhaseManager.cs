using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class WallPhaseManager : MonoBehaviour
{
    public List<GameObject> wallPhases;
    public float phaseSwitchInterval = 20f;

    private int currentPhase = 0;
    private float timer = 0f;

    private NavMeshSurface navMeshSurface;

    void Start()
    {
        navMeshSurface = Object.FindFirstObjectByType<NavMeshSurface>();
        ActivatePhase(currentPhase);
        navMeshSurface.BuildNavMesh();  // Build initial NavMesh
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= phaseSwitchInterval)
        {
            timer = 0f;
            SwitchToNextPhase();
        }
    }

    void SwitchToNextPhase()
    {
        wallPhases[currentPhase].SetActive(false);
        currentPhase = (currentPhase + 1) % wallPhases.Count;
        ActivatePhase(currentPhase);
        navMeshSurface.BuildNavMesh();  // Update NavMesh after switching
    }

    void ActivatePhase(int index)
    {
        for (int i = 0; i < wallPhases.Count; i++)
        {
            wallPhases[i].SetActive(i == index);
        }
    }
}
