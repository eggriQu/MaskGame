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
        player.MaskAbility(mask);
        UIManager.Instance.SetJumpMaskPage(mask);
        Destroy(this.gameObject);
    }
}
