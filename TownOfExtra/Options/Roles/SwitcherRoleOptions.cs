using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Outlier;

namespace TownOfExtra.Options.Roles;

public sealed class SwitcherRoleOptions : AbstractOptionGroup<SwitcherRole>
{
    public override string GroupName => "Switcher";

    [ModdedNumberOption("Switch Cooldown", 5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float SwitchCooldown { get; set; } = 20f;
}