using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementX : MonoBehaviour
{
    [SerializeField]
    Transform[] patrolPoints;
    [SerializeField]
    Animator animator;
    int targetPoint;
    [SerializeField]
    float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) <= 0.3f)
        {
            incrementTargetPoint();
        }

        if (animator != null)
        {
            animator.SetFloat("speed_f", speed);
        }

        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);
        transform.LookAt(patrolPoints[targetPoint].position);
    }
    void incrementTargetPoint()
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }
}
