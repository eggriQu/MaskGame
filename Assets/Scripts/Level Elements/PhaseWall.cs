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
        if (player.hasPhaseMask && player.isDashing && player.playerVelocity != Vector2.zero)
        {
            StartCoroutine(PhaseTimer());
            player.PhaseWallPush(0.65f);
        }
        else if (player.hasPhaseMask && player.isDashing && (player.playerVelocity.x == 0 || player.playerVelocity.y == 0))
        {
            StartCoroutine(PhaseTimer());
            player.PhaseWallPush(1.05f);
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
