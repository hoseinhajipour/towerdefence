using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform traget;
    public NavMeshAgent navMeshAgent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(traget.position);
    }
}
