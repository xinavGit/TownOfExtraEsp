using System.Linq;
using HarmonyLib;
using Reactor.Utilities.Extensions;
using TownOfExtra.Roles.Impostor.Killing;
using TownOfUs.Utilities;

namespace TownOfExtra.Patches;

public class StrikerPatches
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Toggle))]
    public static class ChatTogglePatch
    {
        public static void Postfix(ChatController __instance)
        {
            if (!__instance.IsOpenOrOpening) return;
            if (MeetingHud.Instance) return;
            if (PlayerControl.LocalPlayer.Data.Role is not StrikerRole) return;
            
            __instance.freeChatField.SetVisible(false);
            __instance.quickChatField.SetVisible(false);
            
            foreach (var msg in __instance.chatBubblePool.activeChildren.ToArray()) if (msg != null && msg.gameObject) msg.gameObject.Destroy();
            __instance.chatBubblePool.activeChildren.Clear();

            var dead = StrikerRole.Messages.Keys.Where(x => x.Data.IsDead || x.Data.Disconnected).ToList();
            foreach (var key in dead) StrikerRole.Messages.Remove(key);

            var title = $"{Palette.ImpostorRed.ToTextColor()}Locate Result</color>";
            foreach (var msg in StrikerRole.Messages.Values) MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, msg, false, true);
        }
    }
}