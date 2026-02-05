using Unity.VisualScripting;
using UnityEngine;

public class FalconMask : BaseMaskConcrete
{
    public FalconMask(Mask maskType)
    {
        this.mask = maskType;
    }
    
    public override void OnPickupMask(PlayerController player)
    {
        Debug.Log(mask.name + " collected");
        player.SetFalconSuperJump(true);
        gameManager.SetJumpMaskPage(mask);
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
}
