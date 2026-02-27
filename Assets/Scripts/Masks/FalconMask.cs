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
        player.MaskAbility(mask);
        UIManager.Instance.SetMaskIcon(mask);
        Destroy(this.gameObject);
    }
}
