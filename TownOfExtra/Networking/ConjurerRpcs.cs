using System.Collections;
using MiraAPI.GameOptions;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfExtra.Options.Roles;
using TownOfUs.Networking;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfExtra.Networking;

public class ConjurerRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.ConjurerPlaceRock)]
    public static void RpcPlaceRock(PlayerControl sender, float x, float y)
    {
        Coroutines.Start(SpawnRock(x, y));
    }

    private static IEnumerator SpawnRock(float x, float y)
    {
        var rock = new GameObject("ConjurerRock");
        rock.transform.position = new Vector3(x, y + 2.5f, y / 1000f + 1f);

        var renderer = rock.AddComponent<SpriteRenderer>();
        renderer.sprite = TownOfExtraAssets.ConjurerRockSprite.LoadAsset();

        float velocity = 0f;
        var pos = rock.transform.position;

        while (pos.y > y)
        {
            velocity += 10f * Time.deltaTime;
            pos.y -= velocity * Time.deltaTime;
            pos.y = Mathf.Max(pos.y, y);
            rock.transform.position = pos;
            yield return null;
        }

        try
        {
            var collider = rock.AddComponent<PolygonCollider2D>();
            var points = new Il2CppSystem.Collections.Generic.List<Vector2>();
            renderer.sprite.GetPhysicsShape(0, points);
            collider.SetPath(0, points);
        }
        catch {}
        
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.Data.IsDead) continue;
            if (Vector2.Distance(player.transform.position, new Vector2(x, y)) < 0.5f)
            {
                player.RpcSpecialMurder(player, causeOfDeath: "Crushed");
                break;
            }
        }

        float duration = OptionGroupSingleton<ConjurerRoleOptions>.Instance.ConjureDuration;
        yield return new WaitForSeconds(duration);

        if (rock != null) Object.Destroy(rock);
    }
}