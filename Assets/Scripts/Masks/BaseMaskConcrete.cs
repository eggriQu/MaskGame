using System;
using UnityEngine;
using UnityEngine.Serialization;


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
            if (other.gameObject.GetComponent<PlayerController>() == null)
            {
                OnPickupMask(other.gameObject.GetComponentInParent<PlayerController>());
            }
            else
            {
                OnPickupMask(other.gameObject.GetComponent<PlayerController>());
            }
        }
    }
    
    public virtual void OnPickupMask(PlayerController player)
    {
        Debug.Log(mask.name + "Collected");
    }
}
