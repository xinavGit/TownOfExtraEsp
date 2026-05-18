using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Support;

namespace TownOfExtra.Options.Roles;

public sealed class ConjurerRoleOptions : AbstractOptionGroup<ConjurerRole>
{
    public override string GroupName => "Conjurer";

    [ModdedNumberOption("Conjure Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ConjureCooldown { get; set; } = 30f;

    [ModdedNumberOption("Conjure Duration", 2.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ConjureDuration { get; set; } = 15f;

    [ModdedToggleOption("Can crush impostors")]
    public bool CanCrushImps { get; set; } = true;
}