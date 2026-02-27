using UnityEngine;

public class SkullMask : BaseMaskConcrete
{
    public SkullMask(Mask maskType)
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
