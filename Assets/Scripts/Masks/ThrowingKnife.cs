using UnityEngine;

public class ThrowingKnife : BaseMaskConcrete
{
    public ThrowingKnife(Mask maskType)
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
