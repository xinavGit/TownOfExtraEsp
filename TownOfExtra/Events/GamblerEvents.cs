using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using Reactor.Utilities;
using TownOfExtra.Modifiers.Gambler;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Support;

namespace TownOfExtra.Events;

public class GamblerEvents
{
    [RegisterEvent(11)]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {
        if (!AmongUsClient.Instance.AmHost) return;

        var options = OptionGroupSingleton<GamblerRoleOptions>.Instance;

        if (options.LongerCooldownEnabled)
        {
            foreach (PlayerControl p in PlayerControl.AllPlayerControls)
            {
                if (p.HasModifier<LongerCdModifier>())
                {
                    p.SetKillTimer(p.killTimer + options.CooldownBoost.Value);
                }
            }
        }

        if (options.ShorterCooldownEnabled)
        {
            foreach (PlayerControl p in PlayerControl.AllPlayerControls)
            {
                if (p.HasModifier<ShorterCdModifier>())
                {
                    p.SetKillTimer(p.killTimer - options.CooldownNerf.Value);
                }
            }
        }

        foreach (PlayerControl p in PlayerControl.AllPlayerControls)
        {
            if (p.Data.Role is GamblerRole)
            {
                GamblerRole.ClearGambleEffect(p);
                Coroutines.Start(GamblerRole.ApplyRandomGambleEffect(p));
            }
        }
    }
}