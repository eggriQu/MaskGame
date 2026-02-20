using UnityEngine;

public interface ILevelObject
{
    void OnPlayerContact(PlayerController player);
}

public class BaseObject : MonoBehaviour, ILevelObject
{
    [SerializeField] protected BoxCollider2D objectCollider;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerContact(collision.gameObject.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerContact(collision.gameObject.GetComponentInParent<PlayerController>());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerContact(collision.gameObject.GetComponent<PlayerController>());
        }
    }

    public virtual void OnPlayerContact(PlayerController player)
    {

    }
}
