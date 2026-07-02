using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Support;

public sealed class ObstructorRoleerRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Obstructr";
    public string RoleDescription => "Obstruct the crew's abilities";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorSupport;
    public DoomableType DoomHintType => DoomableType.Default;

    public string GetAdvancedDescription()
    {
        return
            "The Obstructor is an Impostor Support role that can obstruct a player, causing their ability buttons to be disabled next time they try and use them." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.ObstructorRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Obstruct", "Obstruct a player, causing their ability buttons to be disabled next time they try and use them.", TownOfExtraAssets.ObstructorObstructButton)
            };
        }
    }
}