using UnityEngine;

public class PhaseMask : BaseMaskConcrete
{
    public PhaseMask(Mask maskType)
    {
        this.mask = maskType;
    }

    public override void OnPickupMask(PlayerController player)
    {
        player.currentMask = mask;
        player.MaskAbility(mask);
        UIManager.Instance.SetDashMaskPage(mask);
        Destroy(this.gameObject);
    }
}
