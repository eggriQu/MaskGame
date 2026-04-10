using System.Collections;
using UnityEngine;

public class BoxObject : BaseObject
{
    private SpriteRenderer BoxSprite;
    private BoxCollider2D CollisionBox;
    private AudioSource BreakSound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BoxSprite = GetComponent<SpriteRenderer>();
        CollisionBox = GetComponent<BoxCollider2D>();
        BreakSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Die()
    {
        BreakSound.Play();
        BoxSprite.enabled = false;
        CollisionBox.enabled = false;

        yield return new WaitForSeconds(BreakSound.clip.length);

        Destroy(gameObject);
    }



    public override void OnPlayerContact(PlayerController player)
    {
        //base.OnPlayerContact(player);
        if (player.hasSkullMask && !player.isGrounded)
        {
            StartCoroutine(Die());
        }
    }
}
