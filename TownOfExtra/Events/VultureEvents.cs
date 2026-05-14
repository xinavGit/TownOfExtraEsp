using TownOfExtra.Roles.Neutral.Outlier;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;

namespace TownOfExtra.Events;

public class VultureEvents
{
    [RegisterEvent]
    public static void StartGameEventHandler(IntroBeginEvent e)
    {
        VultureRole.DeadBodiesEaten = 0;
    }
}