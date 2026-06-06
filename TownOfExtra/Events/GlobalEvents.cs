using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.LocalSettings;
using TownOfUs;

namespace TownOfExtra.Events;

public class GlobalEvents
{
    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent e)
    {
        TownOfExtraColours.UseBasicCrew =
            LocalSettingsTabSingleton<TownOfUsLocalRoleSettings>.Instance.UseCrewmateTeamColorToggle.Value;
        TownOfExtraAssets.UseBasicCrew =
            LocalSettingsTabSingleton<TownOfUsLocalRoleSettings>.Instance.UseCrewmateTeamColorToggle.Value;
    }
}