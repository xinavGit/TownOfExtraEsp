using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Power;

namespace TownOfExtra.Options.Roles;

public sealed class ConjurerRoleOptions : AbstractOptionGroup<ConjurerRole>
{
    public override string GroupName => "Conjurer";

    [ModdedNumberOption("Conjure Cooldown", 0f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ConjureCooldown { get; set; } = 30f;

    [ModdedNumberOption("Conjure Duration", 2.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ConjureDuration { get; set; } = 15f;

    [ModdedEnumOption("Cannot crush", typeof(ConjurerCantCrushOptions),
        ["No One", "Everyone", "Teammates", "Teammates & Self"])]
    public ConjurerCantCrushOptions ConjurerCantCrush { get; set; } = ConjurerCantCrushOptions.NoOne;
}

public enum ConjurerCantCrushOptions
{
    NoOne,
    Everyone,
    Team,
    SelfAndTeam
}