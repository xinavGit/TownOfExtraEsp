using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using TownOfExtra.Networking.Global;
using TownOfExtra.Options;

namespace TownOfExtra.Modifiers.Excluded;

public sealed class PanicShieldShieldModifier : TimedModifier
{
    public override float Duration => OptionGroupSingleton<CrewmateModifierOptions>.Instance.PanicShieldDuration.Value;
    public override string ModifierName => "Panic Shield (Active)";
    public override bool AutoStart => true;
    public override bool HideOnUi => true;

    public override void OnMeetingStart()
    {
        ModifierComponent?.RemoveModifier(this);
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent?.RemoveModifier(this);
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        if (!Player.AmOwner) return;

        PlayerControl.LocalPlayer.RpcSendNotification(
            $"Your {TownOfExtraColours.PanicShieldModifierColour.ToTextColor()}panic shield</color> has expired!",
            "PanicShieldModifierIcon",
            "CrewModIcon",
            225,
            TownOfExtraColours.PanicShieldModifierColour
        );
    }
}