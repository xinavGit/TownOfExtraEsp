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

namespace TownOfExtra.Modifiers.Game.Impostor.Utility;

public class ShockwaveModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Shockwave";
    public override ModifierFaction FactionType => ModifierFaction.ImpostorUtility;
    public override string IntroInfo => "You can set off a shockwave";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.ShockwaveModifierIcon;
    public Color ModifierColor => TownOfExtraColours.ShockwaveModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.ShockwaveModifierColour;

    public override string GetDescription() => IntroInfo;

    public string GetAdvancedDescription()
    {
        return "You have a button to spawn a shockwave. This causes all nearby players's screens to shake and lower their vision and speed for X seconds.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveChance;
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