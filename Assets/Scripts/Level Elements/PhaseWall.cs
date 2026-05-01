using System.Collections;
using UnityEngine;

public class PhaseWall : BaseObject
{
    [SerializeField] private Material phaseWallMat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //phaseWallMat = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerContact(PlayerController player)
    {
        base.OnPlayerContact(player);
        if (player.isDashing && !objectCollider.isTrigger)
        {
            player.isPhasing = true;
            StartCoroutine(PhaseTimer());
            player.PhaseWallPush(0.8f);
        }
        else if (player.isDashing && objectCollider.isTrigger)
        {
            player.isPhasing = true;
            player.PhaseWallPush(0.8f);
        }
    }

    public override void OnPlayerExit(PlayerController player)
    {
        base.OnPlayerExit(player);
        player.isPhasing = false;
    }

    private IEnumerator PhaseTimer()
    {
        objectCollider.isTrigger = true;
        spriteRenderer.material.SetFloat("_alpha", 0.5f);
        yield return new WaitForSeconds(1.5f);
        spriteRenderer.material.SetFloat("_alpha", 1);
        objectCollider.isTrigger = false;
    }
}
