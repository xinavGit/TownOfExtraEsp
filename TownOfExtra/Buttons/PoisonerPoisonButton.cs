using System.Collections;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Modifiers;
using TownOfExtra.Options;
using TownOfExtra.Roles.Impostor.Killing;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class PoisonerPoisonButton : TownOfUsKillRoleButton<PoisonerRole, PlayerControl>, IKillButton, IDiseaseableButton
{
    public override string Name => "Poison";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.PoisonerRoleColour;
    public override float Cooldown => OptionGroupSingleton<PoisonerRoleOptions>.Instance.PoisonCooldown;
    public static float Delay => OptionGroupSingleton<PoisonerRoleOptions>.Instance.PoisonDelay;
    public static float Length => OptionGroupSingleton<PoisonerRoleOptions>.Instance.PoisonLength;
    public override float EffectDuration => Length + Delay;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.PoisonerPoisonButton;

    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

    public override PlayerControl GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(
            true,
            Distance,
            predicate: plr =>
                plr != null &&
                plr != PlayerControl.LocalPlayer &&
                !plr.HasDied() &&
                !plr.HasModifier<PoisonedModifier>());
    }

    protected override void OnClick()
    {
        if (Target == null) return;
        
        OverrideName("Applying Poison...");
        
        var notif = Helpers.CreateAndShowNotification(
            $"Applying {TownOfExtraColours.PoisonerRoleColour.ToTextColor()}poison</color> to {Target.Data.PlayerName}...",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.PoisonerPoisonButton.LoadAsset());
        notif.AdjustNotification();

        string targetName = Target.Data.PlayerName;
        PlayerControl target = Target;

        Coroutines.Start(Wait(Delay, () =>
        {
            if (target == null || target.Data == null) return;

            target.RpcAddModifier<PoisonedModifier>(PlayerControl.LocalPlayer);

            var pnotif = Helpers.CreateAndShowNotification(
                $"You poisoned {TownOfExtraColours.PoisonerRoleColour.ToTextColor()}{targetName}</color>!",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.PoisonerPoisonButton.LoadAsset());
            pnotif.AdjustNotification();

            OverrideName("Poisoning...");
        }));
    }

    public override void OnEffectEnd()
    {
        OverrideName("Poison");
    }

    private static IEnumerator Wait(float seconds, System.Action onComplete = null)
    {
        yield return new WaitForSeconds(seconds);
        onComplete?.Invoke();
    }
}