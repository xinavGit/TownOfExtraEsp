using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Networking;

public class EraserRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.EraserNotifyErased)]
    public static void RpcNotifyErased(PlayerControl target)
    {
        if (PlayerControl.LocalPlayer != target) return;
        
        Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
        var notif = Helpers.CreateAndShowNotification(
            $"Your role has been erased by the {Palette.ImpostorRed.ToTextColor()}eraser</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.EraserRoleIcon.LoadAsset());
        notif.AdjustNotification();
    }
}