using System;
using UnityEngine;


public interface IConcreteMask
{
    void OnPickupMask(PlayerController player);
}


[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class BaseMaskConcrete : MonoBehaviour, IConcreteMask
{ 
    public Mask mask;
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickupMask(other.gameObject.GetComponent<PlayerController>());
        }
    }
    
    public virtual void OnPickupMask(PlayerController player)
    {
        Debug.Log(mask.name + "Collected");
    }
}
