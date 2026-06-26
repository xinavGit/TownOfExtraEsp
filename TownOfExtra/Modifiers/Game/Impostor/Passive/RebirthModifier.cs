using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers.Game.Impostor.Passive;

public class RebirthModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Rebirth";
    public override ModifierFaction FactionType => ModifierFaction.ImpostorPassive;
    public override string IntroInfo => "Harness a fallen impostor's role";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.RebirthModifierIcon;
    public Color ModifierColor => Palette.ImpostorRed;
    public override Color FreeplayFileColor => Palette.ImpostorRed;

    public static bool Used;

    public override string GetDescription() => IntroInfo;

    public string GetAdvancedDescription()
    {
        return "When your first teammate dies or is ejected, you will be given the option to switch to their role.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.RebirthAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<ImpostorModifierOptions>.Instance.RebirthChance;
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (!base.IsModifierValidOn(role) || role is HaunterRole || role is SpectreRole)
            return false;

        var player = role.Player;
        if (player == null || player.Data == null || player.Data.IsDead || !player.IsImpostor())
            return false;

        return true;
    }
}