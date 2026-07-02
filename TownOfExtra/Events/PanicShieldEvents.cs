using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using Reactor.Utilities;
using TownOfExtra.Modifiers.Excluded;
using TownOfUs;
using TownOfUs.Buttons;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Options;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public static class PanicShieldEvents
{
    [RegisterEvent]
    public static void MiraButtonClickEventHandler(MiraButtonClickEvent e)
    {
        var button = e.Button as CustomActionButton<PlayerControl>;
        var target = button?.Target;

        if (target == null || !button.CanClick())
        {
            return;
        }

        CheckForPanicShield(e, PlayerControl.LocalPlayer, target);
    }

    [RegisterEvent]
    public static void MiraButtonCancelledEventHandler(MiraButtonCancelledEvent e)
    {
        var source = PlayerControl.LocalPlayer;
        var button = e.Button as CustomActionButton<PlayerControl>;
        var target = button?.Target;

        if (target == null || button is not IKillButton)
        {
            return;
        }

        if (target && !target!.HasModifier<PanicShieldShieldModifier>())
        {
            return;
        }

        ResetButtonTimer(source, button);
    }

    [RegisterEvent]
    public static void BeforeMurderEventHandler(BeforeMurderEvent e)
    {
        var source = e.Source;
        var target = e.Target;

        if (CheckForPanicShield(e, source, target))
        {
            ResetButtonTimer(source);
        }
    }

    private static bool CheckForPanicShield(MiraCancelableEvent e, PlayerControl source, PlayerControl target)
    {
        if (MeetingHud.Instance || ExileController.Instance)
        {
            return false;
        }

        if (!target.HasModifier<PanicShieldShieldModifier>() || target == source)
        {
            return false;
        }

        e.Cancel();

        return true;
    }

    private static void ResetButtonTimer(PlayerControl source, CustomActionButton<PlayerControl> button = null)
    {
        if (!source.AmOwner)
        {
            return;
        }

        var reset = OptionGroupSingleton<GeneralOptions>.Instance.TempSaveCdReset;

        button?.SetTimer(reset);

        source.SetKillTimer(reset);
        Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.NeutralWiki, alpha: 0.5f));
    }
}