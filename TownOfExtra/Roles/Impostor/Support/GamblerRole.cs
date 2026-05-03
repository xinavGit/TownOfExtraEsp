using System.Collections;
using System.Collections.Generic;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Modifiers.Gambler;
using TownOfExtra.Options.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Support;

public sealed class GamblerRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Gambler";
    public string RoleDescription => "Get random buffs/nerfs!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorSupport;
    public DoomableType DoomHintType => DoomableType.Trickster;

    public string GetAdvancedDescription()
    {
        return
            "The Gambler is an Impostor Support role that gains a new buff or nerf every kill." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.GamblerRoleIcon
    };

    public override void OnRoleSet()
    {
        if (PlayerControl.LocalPlayer != Player) return;
        Player.RpcAddModifier<NoAbilityModifier>();
    }

    public static void ClearGambleEffect(PlayerControl player)
    {
        player.RpcRemoveModifier<LongerCdModifier>();
        player.RpcRemoveModifier<NoAbilityModifier>();
        player.RpcRemoveModifier<ShorterCdModifier>();
    }
    
    public static IEnumerator ApplyRandomGambleEffect(PlayerControl player)
    {
        yield return new WaitForSeconds(0.1f);
        
        var options = OptionGroupSingleton<GamblerRoleOptions>.Instance;
        var validEffects = new List<int>();

        if (options.LongerCooldownEnabled)
            validEffects.Add(0);

        if (options.ShorterCooldownEnabled)
            validEffects.Add(1);

        if (validEffects.Count == 0)
        {
            player.RpcAddModifier<NoAbilityModifier>();
            yield break;
        }

        int choice = validEffects[System.Random.Shared.Next(validEffects.Count)];

        switch (choice)
        {
            case 0:
                player.RpcAddModifier<LongerCdModifier>();
                break;
            case 1:
                player.RpcAddModifier<ShorterCdModifier>();
                break;
        }
    }
}