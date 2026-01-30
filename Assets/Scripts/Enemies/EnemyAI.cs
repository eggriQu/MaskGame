using System.Collections;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int damage);
}

public interface IMasked
{
    void ChangeMask(int maskType);
}

public class EnemyAI : MonoBehaviour, IDamagable, IMasked
{
    [SerializeField] private PlayerContoller player;
    public Vector3 followDirection;
    private State currentState;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    public bool hostile;
    public bool scared;
    public bool heroic;

    public Mask currentMask;
    [SerializeField] private int maskType;

    public Rigidbody2D rb;

    public void TakeDamage(int damage)
    {
        
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    public void ChangeMask(int maskIndex)
    {
        spriteRenderer.sprite = gameManager.masks[maskIndex].maskSprite;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerContoller>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spriteRenderer.sprite = currentMask.maskSprite;
        StartCoroutine(PatrolRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player").GetComponent<PlayerContoller>();
        followDirection = player.transform.position - transform.position;
        maskType = currentMask.maskType;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentMask.maskType == 1)
            {
                if (player.maskType == 0 || player.maskType == 1)
                {
                    hostile = false;
                    scared = false;
                }
                else if (player.maskType == 2)
                {
                    StopAllCoroutines();
                    ChangeState(new FearState(this));
                    ChangeMask(0);
                    hostile = false;
                    scared = true;
                }
            }
            else if (currentMask.maskType == 2)
            {
                if (player.maskType == 0 || player.maskType == 1)
                {
                    hostile = false;
                    scared = false;
                }
                else if (player.maskType == 2)
                {
                    StopAllCoroutines();
                    if (heroic)
                    {
                        ChangeState(new HostileState(this));
                        ChangeMask(2);
                        hostile = true;
                        scared = false;
                    }
                    else
                    {
                        ChangeState(new FearState(this));
                        ChangeMask(0);
                        hostile = false;
                        scared = true;
                    }
                }
            }
            else if (currentMask.maskType == 3)
            {
                if (player.maskType == 0 || player.maskType == 1)
                {
                    hostile = false;
                    scared = false;
                }
                else if (player.maskType == 2)
                {
                    StopAllCoroutines();
                    ChangeState(new HostileState(this));
                    ChangeMask(2);
                    hostile = true;
                    scared = false;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentMask.maskType == 1)
            {
                if (player.maskType == 0 || player.maskType == 1)
                {
                    hostile = false;
                    scared = false;
                }
                else if (player.maskType == 2)
                {
                    StopAllCoroutines();
                    ChangeState(new FearState(this));
                    ChangeMask(0);
                    hostile = false;
                    scared = true;
                }
            }
            else if (currentMask.maskType == 2)
            {
                if (player.maskType == 0 || player.maskType == 1)
                {
                    hostile = false;
                    scared = false;
                }
                else if (player.maskType == 2)
                {
                    StopAllCoroutines();
                    if (heroic)
                    {
                        ChangeState(new HostileState(this));
                        ChangeMask(2);
                        hostile = true;
                        scared = false;
                    }
                    else
                    {
                        ChangeState(new FearState(this));
                        ChangeMask(0);
                        hostile = false;
                        scared = true;
                    }
                }
            }
            else if (currentMask.maskType == 3)
            {
                if (player.maskType == 0 || player.maskType == 1)
                {
                    hostile = false;
                    scared = false;
                }
                else if (player.maskType == 2)
                {
                    StopAllCoroutines();
                    ChangeState(new HostileState(this));
                    ChangeMask(2);
                    hostile = true;
                    scared = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(PatrolRoutine());
            hostile = false;
            scared = false;
        }
    }
}
