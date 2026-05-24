using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiraAPI.Events;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Utilities;
using TownOfExtra.Events.Custom;
using TownOfExtra.Modifiers;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Outlier;
using TownOfUs.GameOver;
using UnityEngine;

namespace TownOfExtra.Events;

public class PoltergeistEvents
{
    [RegisterEvent]
    public static void OnPoltergeistPossess(TownOfExtraAbilityEvent e)
    {
        TownOfExtraPlugin.Logger.LogInfo("OnPoltergeistPossess received");
        if (e.AbilityType != AbilityType.PoltergeistPossessPlayer) return;

        Coroutines.Start(CheckPoltergeistPossess());
    }
    
    private static IEnumerator CheckPoltergeistPossess()
    {
        yield return new WaitForSeconds(0.25f);

        int possessedCount = 0;

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<PossessedModifier>()) possessedCount++;

            TownOfExtraPlugin.Logger.LogInfo($"OnPoltergeistPossess is now at {possessedCount}");
        }

        TownOfExtraPlugin.Logger.LogInfo($"OnPoltergeistPossess count is finished at {possessedCount}");

        if (possessedCount >= OptionGroupSingleton<PoltergeistRoleOptions>.Instance.WinPossesses)
        {
            TownOfExtraPlugin.Logger.LogInfo("OnPoltergeistPossess possessed count over needed count");

            if (AmongUsClient.Instance.AmHost)
            {
                TownOfExtraPlugin.Logger.LogInfo("OnPoltergeistPossess sent to host for win check");

                List<NetworkedPlayerInfo> winners = new List<NetworkedPlayerInfo>();

                foreach (var poltergeist in CustomRoleUtils
                             .GetActiveRolesOfType<PoltergeistRole>()
                             .Where(t => t.WinConditionMet()))
                {
                    winners.Add(poltergeist.Player.Data);
                }

                CustomGameOver.Trigger<NeutralGameOver>(winners);
            }
        }
    }
}