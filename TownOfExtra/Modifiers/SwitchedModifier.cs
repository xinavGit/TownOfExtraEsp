using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers;

public class SwitchedModifier : BaseModifier
{
    public override string ModifierName => "Switched";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<SwitchedModifier>();
    }
}