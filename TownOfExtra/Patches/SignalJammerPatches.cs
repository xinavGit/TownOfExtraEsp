using System.Linq;
using HarmonyLib;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfExtra.Modifiers;
using TownOfExtra.Networking;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.StartMeeting))]
public static class SjStartMeetingPatch
{
    public static bool Prefix(PlayerControl __instance)
    {
        if (__instance.HasModifier<SignalJammedModifier>())
        {
            SignalJammerRpcs.RpcNotifyOfJAm(__instance);
            
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
public static class SjReportDeadBodyPatch
{
    public static bool Prefix(PlayerControl __instance, NetworkedPlayerInfo target)
    {
        if (__instance.HasModifier<SignalJammedModifier>())
        {
            SignalJammerRpcs.RpcNotifyOfJAm(__instance);

            if (target != null)
            {
                var body = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                if (body != null)
                {
                    body.Reported = false;
                }
            }
            return false;
        }
        return true;
    }
}