using System.Linq;
using TownOfUs.Utilities;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;
using TownOfUs;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modules;
using TownOfUs.Modules.TimeLord;
using TownOfUs.Modules.Components;
using TownOfUs.Options;
using TownOfUs.Options.Roles.Crewmate;
using Object = UnityEngine.Object;

namespace TownOfExtra.Networking;

public class VultureRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.VultureCleanBody, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcCleanBody(PlayerControl player, byte bodyId)
    {
        var body = TimeLordBodyManager.FindDeadBodyIncludingInactive(bodyId);
        if (body == null)
        {
            body = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(x => x.ParentId == bodyId);
        }

        if (body != null)
        {
            var touAbilityEvent = new TouAbilityEvent(AbilityType.JanitorClean, player, body);
            MiraEventManager.InvokeEvent(touAbilityEvent);

            var isHost = AmongUsClient.Instance && AmongUsClient.Instance.AmHost;
            var optionEnabled = OptionGroupSingleton<TimeLordOptions>.Instance.UncleanBodiesOnRewind;
            var destroyBody = (BodyVitalsMode)OptionGroupSingleton<GameMechanicOptions>.Instance.CleanedBodiesAppearance.Value;

            var shouldRecord = isHost ? optionEnabled : (optionEnabled || TimeLordRewindSystem.MatchHasTimeLord());


            if (shouldRecord)
            {
                var bodyPlayer = MiscUtils.PlayerById(bodyId);
                if (bodyPlayer != null)
                {
                    TownOfUs.Events.Crewmate.TimeLordEventHandlers.RecordBodyCleaned(player, body, body.transform.position, 
                        TimeLordBodyManager.CleanedBodySource.Janitor);
                }
                Coroutines.Start(TimeLordBodyManager.CoHideBodyForTimeLord(body, destroyBody));
            }
            else
            {
                Coroutines.Start(body.CoCleanCustom(destroyBody));
            }
            Coroutines.Start(CrimeSceneComponent.CoClean(body));
        }
    }
}