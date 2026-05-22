/*
 * Divani Mods - https://github.com/DivaniNL/TownOfUsMiraDivaniModsAddOn | Divani - https://github.com/DivaniNL
 * For the code for making cleric's cleansing work on TouEx roles
 */

using HarmonyLib;
using TownOfExtra.Utilities;
using TownOfUs.Buttons.Crewmate;

namespace TownOfExtra.Patches;

public static class ClericCleanseExtraEffectsPatch
{
    [HarmonyPatch(typeof(ClericCleanseButton), "OnClick")]
    private static class OnClickPatch
    {
        private static void Postfix(ClericCleanseButton __instance)
        {
            NegativeEffects.CleanseAll(__instance.Target);
        }
    }
}