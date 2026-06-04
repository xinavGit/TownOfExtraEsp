using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class StrikerRoleOptions : AbstractOptionGroup<StrikerRole>
{
    public override string GroupName => "Striker";
    
    [ModdedNumberOption("# of Locate uses per game", 1, 10)]
    public float LocateUses { get; set; } = 5;
    [ModdedNumberOption("# of Locate uses per meeting", 1, 5)]
    public float LocatesPerMeeting { get; set; } = 1;
    [ModdedNumberOption("# of Locate uses per game", 2, 8)]
    public float LocateRoleAmount { get; set; } = 5;
    
    [ModdedNumberOption("Strike Cooldown", 0f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float StrikeCooldown { get; set; } = 10f;
    
    [ModdedNumberOption("Kill Delay", 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ImpendingDoomDuration { get; set; } = 10f;
    [ModdedToggleOption("Announce Doomed")]
    public bool AnnounceDoomed { get; set; } = true;
    public ModdedToggleOption ShowDoomedTimer { get; } =
        new("Show Doomed Timer", false)
        {
            Visible = () => OptionGroupSingleton<StrikerRoleOptions>.Instance.AnnounceDoomed
        };
    
    [ModdedEnumOption("Intro Blurb", typeof(StrikerIntroBlurb),
        ["Normal", "Nerd candy's special blurb"])]
    public StrikerIntroBlurb IntroBlurb { get; set; } = StrikerIntroBlurb.Normal;
}

public enum StrikerIntroBlurb
{
    Normal,
    NerdCandy,
}