using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Support;

namespace TownOfExtra.Options.Roles;

public sealed class GamblerRoleOptions : AbstractOptionGroup<GamblerRole>
{
    public override string GroupName => "Gambler";
    
    [ModdedToggleOption("Longer Cooldown")]
    public bool LongerCooldownEnabled { get; set; } = true;
    public ModdedNumberOption CooldownBoost { get; } =
        new("Cooldown Boost", 5f, 2.5f, 30f, 2.5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<GamblerRoleOptions>.Instance.LongerCooldownEnabled
        };
    [ModdedToggleOption("Shorter Cooldown")]
    public bool ShorterCooldownEnabled { get; set; } = true;
    public ModdedNumberOption CooldownNerf { get; } =
        new("Cooldown Nerf", 5f, 2.5f, 30f, 2.5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<GamblerRoleOptions>.Instance.ShorterCooldownEnabled
        };
}