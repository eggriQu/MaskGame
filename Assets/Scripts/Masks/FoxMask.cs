using UnityEngine;

public class FoxMask : BaseMaskConcrete
{
    public FoxMask(Mask maskType)
    {
        this.mask = maskType;
    }

    public override void OnPickupMask(PlayerController player)
    {
        Debug.Log(mask.name + " collected");
        player.MaskAbility(mask);
        UIManager.Instance.SetSprintMaskPage(mask);
        Destroy(this.gameObject);
    }


}
