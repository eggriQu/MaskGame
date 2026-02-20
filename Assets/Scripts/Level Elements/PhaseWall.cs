using System.Collections;
using UnityEngine;

public class PhaseWall : BaseObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerContact(PlayerController player)
    {
        base.OnPlayerContact(player);
        if (player.hasPhaseMask && player.isDashing)
        {
            StartCoroutine(PhaseTimer());
        }
    }

    private IEnumerator PhaseTimer()
    {
        objectCollider.isTrigger = true;
        spriteRenderer.color = Color.azure; 
        yield return new WaitForSeconds(1.5f);
        spriteRenderer.color = Color.blueViolet;
        objectCollider.isTrigger = false;
    }
}
