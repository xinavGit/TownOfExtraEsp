/*
 * Divani Mods - https://github.com/DivaniNL/TownOfUsMiraDivaniModsAddOn | Divani - https://github.com/DivaniNL
 * For the code for making cleric's cleansing work on TouEx roles
*/

using HarmonyLib;
using TownOfExtra.Utilities;
using TownOfUs.Modifiers.Crewmate;

namespace TownOfExtra.Patches;
public static class ClericCleanseExtraEffectsPatch
{
    [HarmonyPatch(typeof(ClericCleanseModifier), "CleansePlayer")]
    private static class CleansePlayerPatch
    {
        private static void Postfix(ClericCleanseModifier __instance)
        {
            NegativeEffects.CleanseAll(__instance.Player);
        }
    }
}