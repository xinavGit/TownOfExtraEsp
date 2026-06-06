using MiraAPI.Modifiers;
using UnityEngine;

namespace TownOfExtra.Modifiers;

public class InterviewModifier : BaseModifier
{
    public override string ModifierName => "In Interview";
    public override bool HideOnUi => true;
    
    public bool Active { get; set; } = false;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<InterviewModifier>();
    }
    
    public void OnRoundStart()
    {
        if (!Player.AmOwner)
        {
            return;
        }

        HudManager.Instance.Chat.SetVisible(true);
        var buttonArray = new []
            { TownOfExtraAssets.JournalistChatIdle.LoadAsset(), TownOfExtraAssets.JournalistChatHover.LoadAsset(), TownOfExtraAssets.JournalistChatOpen.LoadAsset()};
        HudManager.Instance.Chat.chatButton.transform.Find("Inactive").GetComponent<SpriteRenderer>().sprite = buttonArray[0];
        HudManager.Instance.Chat.chatButton.transform.Find("Active").GetComponent<SpriteRenderer>().sprite = buttonArray[1];
        HudManager.Instance.Chat.chatButton.transform.Find("Selected").GetComponent<SpriteRenderer>().sprite = buttonArray[2];
    }
}