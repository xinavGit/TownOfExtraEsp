using AchievementsAPI.API;
using UnityEngine;

namespace TownOfExtra.Achievements;

public class ToexAchievementsTab : AchievementsTab
{
    public override string Name => "Town of Extra";
    public override bool IsSelectable => true;

    public override Color GetTabColor() => TownOfExtraColours.CreditsColour;
    public override Sprite GetIcon() => TownOfExtraAssets.TownOfExtraIcon.LoadAsset();
    
    // ------------------------------
    //         achievements
    // ------------------------------
    
    public BaseAchievement LaunchGame { get; set; } = new BaseAchievement(
        "Welcome to Town of Extra!", "Launch the game with Town of Extra & Achievements API installed", "TownOfExtra.Resources.Misc.TownOfExtraIcon.png"
    );
    public BaseAchievement DropRockOnPlayer { get; set; } = new BaseAchievement(
        "Splat", "Conjure up a rock and drop it on someone", "TownOfExtra.Resources.Imp.Misc.ConjurerRockSprite.png"
    );
    public BaseAchievement UseRebirth { get; set; } = new BaseAchievement(
        "Reborn", "Rebirth into a dead teammate's role", "TownOfExtra.Resources.Modifiers.Imp.ModifierIcons.RebirthModifierIcon.png"
    );
    public BaseAchievement UseChiefRecruit { get; set; } = new BaseAchievement(
        "Recruiter", "Recruit someone as the chief", "TownOfExtra.Resources.Crew.Buttons.ChiefRecruitButton.png"
    );
    public BaseAchievement UseFreezeAbility { get; set; } = new BaseAchievement(
        "Chilly", "Use the freezer's ability to stop everyone in their tracks", "TownOfExtra.Resources.Imp.Buttons.FreezerFreezeButton.png"
    );
    public BaseAchievement ReportTricksterBody { get; set; } = new BaseAchievement(
        "Tricked", "Report a trickster's fake body", "TownOfExtra.Resources.Neut.Buttons.TricksterPlaceButton.png"
    );
    public BaseAchievement MeetingWhileJammed { get; set; } = new BaseAchievement(
        "No Wifi", "Try and report a body/call a meeting while signals are jammed", "TownOfExtra.Resources.Imp.RoleIcons.SignalJammerRoleIcon.png"
    );
    public BaseAchievement VultureEatBody { get; set; } = new BaseAchievement(
        "Crunchy", "Eat a body as the vulture", "TownOfExtra.Resources.Neut.RoleIcons.VultureRoleIcon.png"
    );
}