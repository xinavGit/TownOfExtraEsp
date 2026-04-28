using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using TownOfExtra.Networking;
using TownOfExtra.Options;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class TricksterPlaceButton : TownOfUsRoleButton<TricksterRole>
{
    public override string Name => "Place Body";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.TricksterRoleColour;
    public override float Cooldown => OptionGroupSingleton<TricksterRoleOptions>.Instance.SampleCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.Placeholder;
    private static float MaxBodies => 1f;
    public override bool CanUse()
    {
        return TricksterRole.HasSampledColour;
    }

    protected override void OnClick()
    {
        if (!TricksterRole.HasSampledColour)
        {
            return;
        }
        if (TricksterRole.SpawnedBodies.Count >= MaxBodies)
        {
            var oldest = TricksterRole.SpawnedBodies[0];
            if (oldest != null)
            {
                Object.Destroy(oldest.gameObject);
            }

            TricksterRole.SpawnedBodies.RemoveAt(0);
        }
        
        var colourName = Palette.GetColorName(TricksterRole.SampledColourId);
        var colour = Palette.PlayerColors[TricksterRole.SampledColourId];
        
        var notif = Helpers.CreateAndShowNotification(
            $"Placed fake body with colour <color=#{colour.ToHtmlStringRGBA()}>{colourName}</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset()
        );
        notif.AdjustNotification();

        static PlayerControl GetTrickster()
        {
            foreach (var p in PlayerControl.AllPlayerControls)
            {
                if (p.IsRole<TricksterRole>())
                {
                    return p;
                }
            }

            return null;
        }

        PlayerControl trickster = GetTrickster();
        if (trickster == null) return;
        TricksterRpcs.RpcPlaceFakeBody(trickster.transform.position, (byte)TricksterRole.SampledColourId, trickster.PlayerId);
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        if (TricksterRole.HasSampledColour)
        {
            var colourName = Palette.GetColorName(TricksterRole.SampledColourId);
            OverrideName($"Place {colourName}");
        }
        else
        {
            OverrideName("Place Body");
        }
    }
    
    // thx to some guy on the reactor discord for this
    public static DeadBody CreateDeadBody(Vector3 position, byte colorId, byte bodyParentId, PlayerControl colorSource)
    {
        if (colorSource?.Data?.DefaultOutfit == null) return null;
        if (GameManager.Instance?.deadBodyPrefab == null || GameManager.Instance.deadBodyPrefab.Length == 0) return null;

        var baseColorId = colorSource.Data.DefaultOutfit.ColorId;
        colorSource.Data.DefaultOutfit.ColorId = colorId;

        var deadBody = Object.Instantiate(GameManager.Instance.deadBodyPrefab[0]);
        deadBody.ParentId = bodyParentId;

        foreach (var bodyRenderer in deadBody.bodyRenderers)
            colorSource.SetPlayerMaterialColors(bodyRenderer);

        colorSource.SetPlayerMaterialColors(deadBody.bloodSplatter);

        var bodyPosition = position + colorSource.KillAnimations[0].BodyOffset;
        bodyPosition.z = bodyPosition.y / 1000f;
        deadBody.transform.position = bodyPosition;

        colorSource.Data.DefaultOutfit.ColorId = baseColorId;

        deadBody.enabled = true;
    
        return deadBody;
    }
}