using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using TownOfUs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Networking;

public class ChiefRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.ChiefNotifiyRecruited)]
    public static void RpcNotifyChiefRecruited(PlayerControl player)
    {
        if (PlayerControl.LocalPlayer != player) return;

        var notif = Helpers.CreateAndShowNotification(
            $"You have been recruited by the {TownOfExtraColours.ChiefRoleColour.ToTextColor()}chief</color>, you are now a {TownOfUsColors.Sheriff.ToTextColor()}sheriff</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.ChiefRoleIcon.LoadAsset());
        notif.AdjustNotification();
    }
}