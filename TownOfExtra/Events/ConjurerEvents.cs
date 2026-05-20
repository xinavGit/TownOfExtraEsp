using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using TownOfExtra.Networking;
using UnityEngine;

namespace TownOfExtra.Events;

public class ConjurerEvents
{
    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent e)
    {
        foreach (var body in Object.FindObjectsOfType<SquashedBody>()) Object.Destroy(body.gameObject);
    }
}