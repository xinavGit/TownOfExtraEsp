using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
using TownOfExtra.Networking;
using TownOfExtra.Roles.Neutral.Outlier;

namespace TownOfExtra.Events;

public class SwitcherEvents
{
    [RegisterEvent(1)]
    public static void RoundStartEventHandler(RoundStartEvent e)
    {
        var switcher = GetSwitcher();

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<SwitchedModifier>())
            {
                if (switcher == null || p == null || p.Data.IsDead || switcher.Data.IsDead)
                {
                    if (switcher != null)
                    {
                        switcher.RpcSendNotification(
                            $"Your {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switcher</color> target is no longer alive!",
                            "SwitcherRoleIcon",
                            TownOfExtraColours.SwitcherRoleColour
                        );
                    }

                    return;
                }

                var pRole = p.Data.Role.Role;
                var switcherRole = switcher.Data.Role.Role;

                p.RpcSetRole(switcherRole);
                switcher.RpcSetRole(pRole);
                p.RpcRemoveModifier<SwitchedModifier>();

                p.RpcSendNotification(
                    $"Your role has been switched with the {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switcher</color>!",
                    "SwitcherRoleIcon",
                    TownOfExtraColours.SwitcherRoleColour
                );
                switcher.RpcSendNotification(
                    $"You have {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switched</color> your role with {p.name}!",
                    "SwitcherRoleIcon",
                    TownOfExtraColours.SwitcherRoleColour
                );
            }
        }
    }

    public static PlayerControl GetSwitcher()
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.Data.Role is SwitcherRole)
            {
                return p;
            }
        }

        return null;
    }
}