using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class StageCollectable : MonoBehaviour
{
    public static Action CollectableCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectableCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}
