using System.Collections;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs.Buttons;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class SquidSpillButton : TownOfUsRoleButton<SquidRole>
{
    public override string Name => "Spill";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.SquidRoleColour;
    public override float Cooldown => OptionGroupSingleton<SquidRoleOptions>.Instance.SpillCooldown;
    public override float EffectDuration => OptionGroupSingleton<SquidRoleOptions>.Instance.InkDuration;
    public override bool HasEffect => true;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.SquidSpillButton;

    private bool _placing;
    private GameObject _preview;

    public override void ClickHandler()
    {
        if (!CanClick()) return;
        OnClick();
    }

    public override bool CanUse()
    {
        return Timer <= 0 && !_placing && !PlayerControl.LocalPlayer.inVent;
    }

    protected override void OnClick()
    {
        Coroutines.Start(StartPlacing());
    }

    private IEnumerator StartPlacing()
    {
        var camera = Camera.main;
        if (camera == null) yield break;

        EnterPlacingMode();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            while (true)
            {
                if (!_placing) yield break;

                if (PlayerControl.LocalPlayer.inVent)
                {
                    ExitPlacingMode();
                    yield return null;
                }

                if (PlayerControl.LocalPlayer.Data.Disconnected || PlayerControl.LocalPlayer.Data.IsDead)
                {
                    ExitPlacingMode();
                    yield return null;
                }

                var screenToWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);

                if (_preview != null)
                {
                    var loc = screenToWorldPoint;
                    loc.z = 0f;
                    _preview.transform.position = loc;
                }

                if (Input.touchCount == 3)
                {
                    ExitPlacingMode();
                    yield break;
                }

                if (Input.touchCount == 1)
                {
                    screenToWorldPoint.z = 0f;

                    SquidRpcs.RpcSpillInk(PlayerControl.LocalPlayer, screenToWorldPoint.x, screenToWorldPoint.y);

                    ExitPlacingMode();

                    EffectActive = true;
                    Timer = EffectDuration;

                    yield break;
                }

                yield return null;
            }
        }
        else
        {
            while (true)
            {
                if (!_placing) yield break;

                if (PlayerControl.LocalPlayer.inVent)
                {
                    ExitPlacingMode();
                    yield return null;
                }

                if (PlayerControl.LocalPlayer.Data.Disconnected || PlayerControl.LocalPlayer.Data.IsDead)
                {
                    ExitPlacingMode();
                    yield return null;
                }

                var screenToWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);

                if (_preview != null)
                {
                    var loc = screenToWorldPoint;
                    loc.z = 0f;
                    _preview.transform.position = loc;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ExitPlacingMode();
                    yield break;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    screenToWorldPoint.z = 0f;

                    SquidRpcs.RpcSpillInk(PlayerControl.LocalPlayer, screenToWorldPoint.x, screenToWorldPoint.y);

                    ExitPlacingMode();

                    EffectActive = true;
                    Timer = EffectDuration;

                    yield break;
                }

                yield return null;
            }
        }
    }

    private void EnterPlacingMode()
    {
        OverrideName("Spilling...");
        _placing = true;

        Vector3 loc = Camera.main != null ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Vector3.zero;

        loc.z = 0f;

        _preview = new GameObject();
        _preview.transform.position = loc;

        var renderer = _preview.AddComponent<SpriteRenderer>();
        renderer.sprite = TownOfExtraAssets.SquidInkPuddle.LoadAsset();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.5f);
    }

    private void ExitPlacingMode()
    {
        OverrideName("Spill");
        _placing = false;

        if (_preview != null)
        {
            Object.Destroy(_preview);
            _preview = null;
        }
    }

    public override void OnEffectEnd()
    {
        Timer = Cooldown;
    }
}