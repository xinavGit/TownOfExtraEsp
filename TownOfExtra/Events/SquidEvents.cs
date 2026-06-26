using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using TownOfExtra.Buttons;
using TownOfExtra.Events.Custom;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfUs;

namespace TownOfExtra.Events;

public class SquidEvents
{
    [RegisterEvent]
    public static void OnRoleButtonUse(MiraButtonClickEvent e)
    {
        if (PlayerControl.LocalPlayer.HasModifier<SlippedModifier>())
        {
            e.Cancel();
            PlayerControl.LocalPlayer.RpcSendNotification(
                $"You cannot use this ability while {TownOfUsColors.Medic.ToTextColor()}recovering</color> from your {TownOfExtraColours.SquidRoleColour.ToTextColor()}slip</color>!",
                "SquidInkPuddle",
                "NeutMisc",
                225
            );
        }
    }
    
    [RegisterEvent]
    public static void OnInkDestroy(TownOfExtraAbilityEvent e)
    {
        if (e.AbilityType != AbilityType.SquidInkDestroyed) return;
        if (PlayerControl.LocalPlayer != e.Player) return;
        
        if (e.Target is PlayerControl target)
        {
            var clr = TownOfExtraColours.SquidRoleColour.ToTextColor();
            PlayerControl.LocalPlayer.RpcSendNotification(
                $"{target.Data.PlayerName} has slipped in your {clr}ink</color>!",
                "SquidInkPuddle",
                "NeutMisc",
                225,
                TownOfExtraColours.SquidRoleColour
            );
        }
        
        var btn = CustomButtonSingleton<SquidSpillButton>.Instance;
        btn.EffectActive = false;
        btn.ResetCooldownAndOrEffect();
    }
}