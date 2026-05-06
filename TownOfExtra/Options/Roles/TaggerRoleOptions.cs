using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class TaggerRoleOptions : AbstractOptionGroup<TaggerRole>
{
    public override string GroupName => "Tagger";
    
    [ModdedNumberOption("Kill Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 40f;
    [ModdedNumberOption("Kill Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MarkCooldown { get; set; } = 25f;
}