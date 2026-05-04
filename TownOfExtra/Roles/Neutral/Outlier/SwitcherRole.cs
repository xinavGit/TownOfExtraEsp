using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Outlier;

public sealed class SwitcherRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Switcher";
    public string RoleDescription => "Switch roles with another player!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.SwitcherRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralOutlier;
    public DoomableType DoomHintType => DoomableType.Trickster;
    
    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = $"{TownOfExtraColours.SwitcherRoleColour.ToTextColor()}Switch your role with another player!</color>\n{TownOfExtraColours.SwitcherRoleColour.ToTextColor()}Optional Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }

    public string GetAdvancedDescription()
    {
        return
            "The Switcher is a Neutral Outlier role that can switch their role with another player, applying after the next meeting." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        TasksCountForProgress = false,
        Icon = TownOfExtraAssets.SwitcherRoleIcon,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Switch", "Switch your role with another player.", TownOfExtraAssets.SwitcherSwitchButton)
            };
        }
    }
}