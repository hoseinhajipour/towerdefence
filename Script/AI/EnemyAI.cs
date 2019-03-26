using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform traget;
    public NavMeshAgent navMeshAgent;

    void Update()
    {
        navMeshAgent.SetDestination(traget.position);
    }
}
