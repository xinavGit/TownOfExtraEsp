using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Crewmate.Power;

public sealed class JournalistRole : CrewmateRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Journalist";
    public string RoleDescription => "Interview players to gain their information";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.JournalistRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmatePower;
    public DoomableType DoomHintType => DoomableType.Insight;
    
    public string GetAdvancedDescription()
    {
        return
            "The Journalist is a Crewmate Power role who successes role that can recruit players, converting them into sheriffs, aswell as being able to shoot players with a limited amount of shots, killing and revealing their roles." +
            MiscUtils.AppendOptionsText(GetType());
    }
    
    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.JournalistRoleIcon
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Interview", "Interview a player, letting them send you one piece of information from the game.", TownOfExtraAssets.JournalistInterviewButton),
            };
        }
    }
    
    public void OnRoundStart()
    {
        if (!Player.AmOwner)
        {
            return;
        }

        HudManager.Instance.Chat.SetVisible(true);
        var buttonArray = new []
            { TownOfExtraAssets.JournalistChatIdle.LoadAsset(), TownOfExtraAssets.JournalistChatHover.LoadAsset(), TownOfExtraAssets.JournalistChatOpen.LoadAsset()};
        HudManager.Instance.Chat.chatButton.transform.Find("Inactive").GetComponent<SpriteRenderer>().sprite = buttonArray[0];
        HudManager.Instance.Chat.chatButton.transform.Find("Active").GetComponent<SpriteRenderer>().sprite = buttonArray[1];
        HudManager.Instance.Chat.chatButton.transform.Find("Selected").GetComponent<SpriteRenderer>().sprite = buttonArray[2];
    }
}