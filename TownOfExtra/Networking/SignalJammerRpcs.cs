using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfExtra.Patches;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Networking;

public class SignalJammerRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.SignalJammerNotifyOfJam)]
    public static void RpcNotifyOfJAm(PlayerControl target)
    {
        if (PlayerControl.LocalPlayer == target)
        {
            Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
            var notif = Helpers.CreateAndShowNotification(
                $"Your meeting signals are {Palette.ImpostorRed.ToTextColor()}jammed</color>!",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.SignalJammerJamButton.LoadAsset());
            notif.AdjustNotification();
        }
    }
}