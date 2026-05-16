using System.Reflection;
using MiraAPI.LocalSettings;
using TownOfUs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra;

public class TownOfExtraColours
{
    public static bool UseBasic { get; set; } =
        LocalSettingsTabSingleton<TownOfUsLocalRoleSettings>.Instance.UseCrewmateTeamColorToggle.Value;
    
    
    
    public static Color ChiefRoleColour => UseBasic ? Palette.CrewmateBlue : new Color32(0, 118, 1, 255);
    
    
    
    public static Color PoisonColour => new Color(46/255f, 82/255f, 53/255f);
    public static Color FreezerRoleColour => new Color(0/255f, 200/255f, 255/255f);
    public static Color CannibalColour => new Color(180/255f, 90/255f, 50/255f);
    
    
    
    public static Color SwitcherRoleColour => new Color(255/255f, 239/255f, 168/255f);
    public static Color TricksterRoleColour => new Color(128 / 255f, 0 / 255f, 155 / 255f);
    public static Color VultureRoleColour => new Color(79 /255f, 24 /255f, 0 /255f);
    
    
    
    public static Color HeavyWorkloadModifierColour => new Color(216 / 255f, 108 / 255f, 2 / 255f);
    public static Color FragileModifierColour => new Color(71 / 255f, 102 / 255f, 125 / 255f);



    public static Color GetRoleColour(string name)
    {
        var touColour = 
            name != null
                ? MiscUtils.GetRoleColour(name)
                : Color.white;
        var roleColour = 
            touColour == TownOfUsColors.Impostor 
                ? GetTownOfExtraRoleColour(name)
                : touColour;

        return roleColour;
    }
    
    public static Color GetTownOfExtraRoleColour(string name)
    {
        var pInfo = typeof(TownOfExtraColours).GetProperty($"{name}RoleColour", BindingFlags.Public | BindingFlags.Static);

        if (pInfo == null)
        {
            return TownOfUsColors.Impostor;
        }

        var colour = (Color)pInfo.GetValue(null)!;

        return colour;
    }
}