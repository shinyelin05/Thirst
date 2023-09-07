using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tset : MonoBehaviour
{
    NavMeshAgent agent;


    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    public void goTarget()
    {
        agent.SetDestination(transform.position);
    }
}
