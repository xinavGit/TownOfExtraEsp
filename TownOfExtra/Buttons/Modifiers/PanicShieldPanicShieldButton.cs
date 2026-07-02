using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Modifiers.Game.Crewmate.Utility;
using TownOfExtra.Networking.Global;
using TownOfExtra.Options;
using TownOfUs.Buttons;
using UnityEngine;

namespace TownOfExtra.Buttons.Modifiers;

public sealed class PanicShieldPanicShieldButton : TownOfUsButton
{
    public override string Name => "Panic Shield";
    public override BaseKeybind Keybind => Keybinds.ModifierAction;
    public override Color TextOutlineColor => TownOfExtraColours.PanicShieldModifierColour;
    public override float Cooldown => 0.01f;
    public override float EffectDuration => OptionGroupSingleton<CrewmateModifierOptions>.Instance.PanicShieldDuration.Value;
    public override int MaxUses => 1;
    public override ButtonLocation Location => ButtonLocation.BottomLeft;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.PanicShieldPanicShieldButton;

    public override bool Enabled(RoleBehaviour role)
    {
        return PlayerControl.LocalPlayer &&
               PlayerControl.LocalPlayer.HasModifier<PanicShieldModifier>() &&
               !PlayerControl.LocalPlayer.Data.IsDead;
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.RpcAddModifier<PanicShieldShieldModifier>();
        
        PlayerControl.LocalPlayer.RpcSendNotification(
            $"You have activated your {TownOfExtraColours.PanicShieldModifierColour.ToTextColor()}panic shield</color>!",
            "PanicShieldModifierIcon",
            "CrewModIcon",
            225,
            TownOfExtraColours.PanicShieldModifierColour
        );
    }
}