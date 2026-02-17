using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class LevelExit : MonoBehaviour
{
  [SerializeField] private Sprite OnSprite;
  private bool isActive;
  private BoxCollider2D collider;
  private SpriteRenderer spriteRenderer;

  public static Action OnLevelExit;
  
  private void OnEnable()
  {
    StageCollectable.CollectableCollected += ActivateExit;
  }

  private void OnDisable()
  {
    StageCollectable.CollectableCollected -= ActivateExit;
  }

  private void Awake()
  {
    collider = GetComponent<BoxCollider2D>();
    collider.isTrigger = true;
    collider.enabled = false;
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log("Level ended");
    OnLevelExit?.Invoke();
    //make end level UI appear
    //halt rest of level
    //save best score?
  }

  private void ActivateExit()
  {
    if (!isActive)
    {
      isActive = true;
      spriteRenderer.sprite = OnSprite;
      collider.enabled = true;
    }
  }
}
