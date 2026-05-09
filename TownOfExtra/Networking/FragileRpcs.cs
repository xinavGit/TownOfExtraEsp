using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;
using TownOfExtra.Modifiers.Game.Crewmate.Passive;
using TownOfExtra.Options;
using TownOfUs.Networking;
using TownOfUs.Utilities;

namespace TownOfExtra.Networking;

public class FragileRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.TriggerFragileModifier, LocalHandling = RpcLocalHandling.None)]
    public static void RpcTriggerFragileModifier(PlayerControl target)
    {
        if (target.AmOwner && target.HasModifier<FragileModifier>() && !target.Data.IsDead)
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.FragileModifierColour));
            
            if (!FragileModifier.Interactions.ContainsKey(target))
            {
                FragileModifier.Interactions.Add(target, 0);
            }
            FragileModifier.Interactions[target] += 1;

            if (FragileModifier.Interactions[target] >=
                OptionGroupSingleton<CrewmateModifierOptions>.Instance.FragileMaxInteractions.Value)
            {
                target.RpcSpecialMurder(target, MeetingCheck.OutsideMeeting, true, true, causeOfDeath: "Shattered");
                FragileModifier.Interactions.Remove(target);
            }
        }
    }
}