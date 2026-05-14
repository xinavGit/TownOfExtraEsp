using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using TownOfExtra.Modifiers.Game.Universal.Passive;
using TownOfUs.Modules;

namespace TownOfExtra.Events;

public static class SoullessEvents
{
    [RegisterEvent]
    public static void BeforeMurderEventHandler(BeforeMurderEvent e)
    {
        var target = e.Target;
        
        if (!target.HasModifier<SoullessModifier>() || !MeetingHud.Instance) return;
        
        e.Cancel();
    }
    
    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent e)
    {
        var target = e.Target;
        var source = e.Source;

        if (target.HasModifier<SoullessModifier>())
        {
            target.RpcRemoveModifier<SoullessModifier>();
            _ = new FakePlayer(target);
            source.RpcCustomMurder(target, createDeadBody:false);
        }
    }
}