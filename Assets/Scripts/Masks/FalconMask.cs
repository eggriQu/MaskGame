using Unity.VisualScripting;
using UnityEngine;

public class FalconMask : BaseMaskConcrete
{
    public FalconMask(string maskName)
    {
        this.maskName = maskName;
    }
    
    public override void OnPickupMask(PlayerController player)
    {
        Debug.Log(maskName + " collected");
        player.SetFalconSuperJump(true);
        Destroy(this.gameObject);
    }
}
