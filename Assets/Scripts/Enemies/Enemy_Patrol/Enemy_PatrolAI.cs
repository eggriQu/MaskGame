using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_PatrolAI : MonoBehaviour
{
    [SerializeField] List<Transform> patrolPoints;
    [SerializeField] float patrolSpeed = 5f;

    private int listIndex = 0;

    private readonly float destinationTolerance = 0.1f;
    private Transform currentTargetDest;

    private void Start()
    {
        if (patrolPoints.Count > 1)
        {
            currentTargetDest = patrolPoints[listIndex];
        }
    }


    private void Update()
    {
        if (patrolPoints.Count > 1)
        {
            if (Vector3.Distance(transform.position, currentTargetDest.position) < destinationTolerance)
            {
                SelectNextPoint();
            }
            MoveToPoint();
        }
    }


    private void SelectNextPoint()
    {
        listIndex = (listIndex + 1) % patrolPoints.Count;
        currentTargetDest = patrolPoints[listIndex];
    }
    
    private void MoveToPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTargetDest.position, Time.deltaTime * patrolSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Kill player, not yet implemented");
        }
    }
    
}
