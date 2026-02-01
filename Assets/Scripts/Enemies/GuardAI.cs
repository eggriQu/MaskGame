using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public interface IInteractable
{
    void TakeDamage(int damage);
    void Interact();
}

public interface IMasked
{
    void ChangeMask(int maskType);
}

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected PlayerContoller player;
    public Vector3 followDirection;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected GameManager gameManager;
    public Rigidbody2D rb;

    public Mask currentMask;
    [SerializeField] protected int maskType;
    [SerializeField] protected bool isMasked;

    protected State currentState;

    public virtual void Update()
    {
        player = GameObject.Find("Player").GetComponent<PlayerContoller>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        followDirection = player.transform.position - transform.position;
        maskType = currentMask.maskType;
    }

    public virtual void ReactToPlayer(bool isExitCollision)
    {

    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }
}

public class GuardAI : Entity, IInteractable, IMasked
{
    public bool hostile;
    public bool scared;
    public bool heroic;

    public void TakeDamage(int damage)
    {

    }

    public void Interact()
    {

    }

    public void ChangeMask(int maskIndex)
    {
        spriteRenderer.sprite = gameManager.masks[maskIndex].maskSprite;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer.sprite = currentMask.maskSprite;
        StartCoroutine(PatrolRoutine());
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
                ChangeState(new HostileState(this));
                ChangeMask(2);
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
