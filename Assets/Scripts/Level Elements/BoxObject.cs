using UnityEngine;

public class BoxObject : BaseObject
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
        if (player.hasSkullMask && !player.isGrounded)
        {
            Destroy(gameObject);
        }
    }
}
