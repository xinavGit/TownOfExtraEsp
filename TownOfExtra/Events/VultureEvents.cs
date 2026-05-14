using TownOfExtra.Roles.Neutral.Outlier;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public class VultureEvents
{
    [RegisterEvent]
    public static void StartGameEventHandler(IntroBeginEvent e)
    {
        VultureRole.DeadBodiesEaten = 0;
    }

    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent e)
    {
        CheckAndConvertVulture();
    }

    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent e)
    {
        CheckAndConvertVulture();
    }

    private static void CheckAndConvertVulture()
    {
        if (!OptionGroupSingleton<VultureRoleOptions>.Instance.TurnIntoAmne) return;
        
        int impostors = 0;
        int others = 0;

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.Data.IsDead || p.GetTownOfUsRole() is VultureRole) continue;
            if (p.IsImpostor()) impostors++;
            else others++;
        }

        VultureRpcs.RpcChangeVultureToAmne(others, impostors);
    }
}