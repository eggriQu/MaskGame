using System.Collections;
using UnityEngine;

public class BouncePad : BaseObject
{
    [SerializeField] private Vector2 bounceDirection;
    [SerializeField] private float bounceForce;

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
        StartCoroutine(BounceTimer(player));
        player.BouncePad(bounceDirection, bounceForce);
        player.isJumping = true;
    }

    private IEnumerator BounceTimer(PlayerController player)
    {
        player.usedBounce = true;
        yield return new WaitForSeconds(0.3f);
        player.usedBounce = false;
    }
}
