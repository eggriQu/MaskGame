using UnityEngine;

public class PhaseMask : BaseMaskConcrete
{
    public PhaseMask(Mask maskType)
    {
        this.mask = maskType;
    }

    public override void OnPickupMask(PlayerController player)
    {
        Debug.Log(mask.name + " collected");
        player.MaskAbility(mask);
        gameManager.SetDashMaskPage(mask);
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
}
