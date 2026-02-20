using UnityEngine;

public class SkullMask : BaseMaskConcrete
{
    public SkullMask(Mask maskType)
    {
        this.mask = maskType;
    }

    public override void OnPickupMask(PlayerController player)
    {
        player.currentMask = mask;
        player.MaskAbility(mask);
        UIManager.Instance.SetJumpMaskPage(mask);
        Destroy(this.gameObject);
    }
}
