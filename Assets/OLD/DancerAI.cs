using System.Collections;
using UnityEngine;

public class DancerAI : Entity, IInteractable, IMasked
{
    public bool hostile;
    public bool scared;
    public bool heroic;
    private State provokedState;
    private int chosenMask;

    public void TakeDamage(int damage)
    {

    }

    public void Interact()
    {

    }

    public void ChangeMask(int maskIndex)
    {
        spriteRenderer.sprite = uiManager.masks[maskIndex].maskSprite;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer.sprite = currentMask.maskSprite;
        StartCoroutine(PatrolRoutine());
        if (Random.Range(0, 2) == 0)
        {
            heroic = false;
            chosenMask = 0;
            provokedState = new FearState(this);
        }
        else
        {
            heroic = true;
            chosenMask = 2;
            provokedState = new HostileState(this);
        }
    }

    void FixedUpdate()
    {
        currentState?.Handle();
    }

    IEnumerator PatrolRoutine()
    {
        spriteRenderer.sprite = currentMask.maskSprite;
        while (true)
        {
            ChangeState(new IdleState(this));
            yield return new WaitForSeconds(2);
            ChangeState(new PatrolState(this));
            yield return new WaitForSeconds(2);
        }
    }

    public override void ReactToPlayer(bool exitCollision)
    {
        base.ReactToPlayer(default);
        if (!exitCollision)
        {
            if (player.maskType == 0 || player.maskType == 1)
            {
                //hostile = false;
            }
            else if (player.maskType == 2)
            {
                ChangeState(provokedState);
                ChangeMask(chosenMask);
                hostile = true;
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(PatrolRoutine());
            hostile = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ReactToPlayer(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ReactToPlayer(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ReactToPlayer(true);
        }
    }
}