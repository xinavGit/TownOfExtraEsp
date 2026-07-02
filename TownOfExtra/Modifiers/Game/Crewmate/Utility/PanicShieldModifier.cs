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

namespace TownOfExtra.Modifiers.Game.Crewmate.Utility;

public class PanicShieldModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Panic Shield";
    public override ModifierFaction FactionType => ModifierFaction.CrewmateUtility;
    public override string IntroInfo => "You can set off a panic shield";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.PanicShieldModifierIcon;
    public Color ModifierColor => TownOfExtraColours.PanicShieldModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.PanicShieldModifierColour;

    public override string GetDescription()
    {
        return $"You can use a temporary 1 time panic shield";
    }

    public string GetAdvancedDescription()
    {
        return $"Only once in the game, you can set off a panic shield protecting you from all attacks for {OptionGroupSingleton<CrewmateModifierOptions>.Instance.PanicShieldDuration.Value} seconds.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.PanicShieldAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<CrewmateModifierOptions>.Instance.PanicShieldChance;
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (!base.IsModifierValidOn(role) || role is HaunterRole || role is SpectreRole)
            return false;

        var player = role.Player;
        if (player == null || player.Data == null || player.Data.IsDead || !role.IsCrewmate())
            return false;

        return true;
    }
}