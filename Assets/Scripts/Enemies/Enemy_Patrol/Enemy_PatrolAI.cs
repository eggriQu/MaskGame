using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_PatrolAI : MonoBehaviour, ILevelObject
{
    [SerializeField] List<Transform> patrolPoints;
    [SerializeField] float patrolSpeed = 5f;

    [SerializeField] private bool stunned;
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider2D boxCollider;

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
        if (PauseManager.isGamePaused || PauseManager.isLevelPaused) return;
        if (patrolPoints.Count > 1 && !stunned)
        {
            if (Vector3.Distance(transform.position, currentTargetDest.position) < destinationTolerance)
            {
                SelectNextPoint();
            }
            MoveToPoint();
        }
        else if (stunned)
        {

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

    private IEnumerator StunRoutine()
    {
        stunned = true;
        anim.Play("Enemy_Stunned");
        boxCollider.isTrigger = true;
        yield return new WaitForSeconds(4);
        boxCollider.isTrigger = false;
        anim.Play("Enemy_Alive");
        stunned = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile") && !stunned)
        {
            StartCoroutine(StunRoutine());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerContact(collision.gameObject.GetComponent<PlayerController>());
        }
    }

    public virtual void OnPlayerContact(PlayerController player)
    {
        if (!player.isDead && !stunned)
        {
            StartCoroutine(player.Die());
        }
    }
}
