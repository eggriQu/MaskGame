using UnityEngine;

public class FoxMask : BaseMaskConcrete
{
    public FoxMask(Mask maskType)
    {
        this.mask = maskType;
    }

    public override void OnPickupMask(PlayerController player)
    {
        player.currentMask = mask;
        player.MaskAbility(mask);
        UIManager.Instance.SetSprintMaskPage(mask);
        Destroy(this.gameObject);
    }
}
