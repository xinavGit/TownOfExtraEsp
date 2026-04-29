using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
public class PnMeetingHudStartPatch
{
    public static void Postfix()
    {
        foreach (PlayerControl p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<PoisonedModifier>())
            {
                var modifier = p.GetModifier<PoisonedModifier>();
                if (modifier == null)
                {
                    return;
                }
                
                modifier.StopTimer();
            }
        }
    }
}