using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    private Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
            if (enemyAnimator != null)
            {
                enemyAnimator.SetFloat("speed_f", navMeshAgent.speed);
            }
        }
    }
}
