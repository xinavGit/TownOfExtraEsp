using HarmonyLib;
using MiraAPI.GameOptions;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Killing;
using TownOfExtra.Roles.Impostor.Power;
using TownOfUs.Modifiers.Game;

namespace TownOfExtra.Patches;

public class AssassinPatches
{
    [HarmonyPatch(typeof(AssassinModifier), nameof(AssassinModifier.IsModifierValidOn))]
    public static class AssasinModifierValidRolesPatch
    {
        public static void Postfix(RoleBehaviour role, ref bool __result)
        {
            if (role is StrikerRole) __result = false;
            if (role is EraserRole && OptionGroupSingleton<EraserRoleOptions>.Instance.CanBeAssassin) __result = false;
        }
    }
}