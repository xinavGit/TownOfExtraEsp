using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Power;

namespace TownOfExtra.Options.Roles;

public sealed class DreamCasterRoleOptions : AbstractOptionGroup<DreamCasterRole>
{
    public override string GroupName => "Dream Caster";

    [ModdedNumberOption("Cast Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float CastCooldown { get; set; } = 30f;

    [ModdedNumberOption("Cast Duration (rounds)", 1f, 10f)]
    public float CastDuration { get; set; } = 2f;
}