using Il2CppSystem;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Options;
using TownOfUs.Networking;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers;

public sealed class PoisonedModifier(PlayerControl poisoner) : TimedModifier
{
    public override string ModifierName => "Poisoned";
    public override float Duration => OptionGroupSingleton<PoisonerRoleOptions>.Instance.PoisonLength;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.PoisonerRoleIcon;

    public static bool ShowPoison => OptionGroupSingleton<PoisonerRoleOptions>.Instance.ShowPoison;

    public override string GetDescription()
    {
        return $"Time until death: {TimeRemaining:F1}s";
    }

    public override void OnActivate()
    {
        if (Player != PlayerControl.LocalPlayer) return;

        if (ShowPoison)
        {
            Player.cosmetics.SetOutline(true, new Nullable<Color>(TownOfExtraColours.PoisonerRoleColour));
        }
        Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.PoisonerRoleColour, Duration));
        var notif = Helpers.CreateAndShowNotification(
            $"You have been {TownOfExtraColours.PoisonerRoleColour.ToTextColor()}poisoned</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.PoisonerRoleIcon.LoadAsset());
        notif.AdjustNotification();
    }

    public override void OnTimerComplete()
    {
        poisoner.RpcSpecialMurder(
            Player,
            isIndirect: true,
            ignoreShield: true,
            teleportMurderer: false,
            showKillAnim: false,
            playKillSound: true,
            causeOfDeath: "Poisoned"
        );

        if (Player == PlayerControl.LocalPlayer)
        {
            var notif = Helpers.CreateAndShowNotification(
                $"You have been {TownOfExtraColours.PoisonerRoleColour.ToTextColor()}poisoned</color> to {Palette.ImpostorRed.ToTextColor()}death</color>!",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.PoisonerRoleIcon.LoadAsset());
            notif.AdjustNotification();
        }

        if (poisoner == PlayerControl.LocalPlayer)
        {
            var poisonerNotif = Helpers.CreateAndShowNotification(
                $"Your {TownOfExtraColours.PoisonerRoleColour.ToTextColor()}poison</color> has {Palette.ImpostorRed.ToTextColor()}killed</color> {Player.Data.PlayerName}!",
                Color.white, new Vector3(0f, 1.8f, -20f), spr: TownOfExtraAssets.PoisonerRoleIcon.LoadAsset());
            poisonerNotif.AdjustNotification();
        }
    }

    public override void OnDeactivate()
    {
        if (Player != PlayerControl.LocalPlayer) return;

        if (ShowPoison)
        {
            Player.cosmetics.SetOutline(false);
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}