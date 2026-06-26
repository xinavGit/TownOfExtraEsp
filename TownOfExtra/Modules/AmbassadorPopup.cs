using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using TMPro;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Modules.Localization;
using TownOfUs.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace TownOfExtra.Modules;

[RegisterInIl2Cpp]
[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Unity")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Unity")]
[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class AmbassadorSelectionMinigame(IntPtr cppPtr) : Minigame(cppPtr)
{
    public Transform RolesHolder;
    public GameObject RolePrefab;
    public TextMeshPro StatusText;
    public TextMeshPro RoleName;
    public SpriteRenderer RoleIcon;
    public TextMeshPro RoleTeam;
    public GameObject RedRing;
    public GameObject WarpRing;

    private readonly Color _bgColor = new Color32(24, 0, 0, 215);
    private RoleTypes? _selectedRole;
    private List<RoleBehaviour> availableRoles = [];
    private Action<RoleBehaviour> clickHandler;
    public static int CurrentCard { get; set; }
    public static int RoleCount { get; set; }

    public string HeaderText = null;
    public string DefaultRoleNameText = null;
    public string DefaultTeamText = null;
    public string RandomCardLabel = null;
    public string RandomCardTeamLabel = null;

    private void Awake()
    {
        if (Instance)
        {
            Instance.Close();
        }

        RolesHolder = transform.FindChild("Roles");
        RolePrefab = transform.FindChild("RoleCardHolder").gameObject;
        StatusText = transform.FindChild("Status").gameObject.GetComponent<TextMeshPro>();
        RoleName = transform.FindChild("Status").FindChild("RoleName").gameObject.GetComponent<TextMeshPro>();
        RoleTeam = transform.FindChild("Status").FindChild("RoleTeam").gameObject.GetComponent<TextMeshPro>();
        RoleIcon = transform.FindChild("Status").FindChild("RoleImage").gameObject.GetComponent<SpriteRenderer>();
        RedRing = transform.FindChild("Status").FindChild("RoleRing").gameObject;
        WarpRing = transform.FindChild("Status").FindChild("RingWarp").gameObject;
        RoleTeam = transform.FindChild("Status").FindChild("RoleTeam").gameObject.GetComponent<TextMeshPro>();

        StatusText.font = HudManager.Instance.TaskPanel.taskText.font;
        StatusText.fontMaterial = HudManager.Instance.TaskPanel.taskText.fontMaterial;
        StatusText.text = TouLocale.Get("TouRoleAmbassadorChooseRole");
        StatusText.gameObject.SetActive(false);

        RoleName.font = HudManager.Instance.TaskPanel.taskText.font;
        RoleName.fontMaterial = HudManager.Instance.TaskPanel.taskText.fontMaterial;
        RoleName.text = TouLocale.Get("Random");
        RoleName.gameObject.SetActive(false);

        RoleTeam.font = HudManager.Instance.TaskPanel.taskText.font;
        RoleTeam.fontMaterial = HudManager.Instance.TaskPanel.taskText.fontMaterial;
        RoleTeam.text = TouLocale.Get("TouRoleAmbassadorRandomImpostorOption");
        RoleTeam.gameObject.SetActive(false);

        RoleIcon.sprite = TouRoleIcons.RandomImp.LoadAsset();
        RoleIcon.gameObject.SetActive(false);
        RedRing.SetActive(false);
        WarpRing.SetActive(false);
    }

    public static AmbassadorSelectionMinigame Create()
    {
        var gameObject = Instantiate(TouAssets.AltRoleSelectionGame.LoadAsset(), HudManager.Instance.transform);
        gameObject.GetComponent<Minigame>().DestroyImmediate();
        gameObject.SetActive(false);
        return gameObject.AddComponent<AmbassadorSelectionMinigame>();
    }

    [HideFromIl2Cpp]
    public void Open(List<RoleBehaviour> roles, Action<RoleBehaviour> onClick, RoleTypes? defaultRole = null)
    {
        availableRoles = roles;
        clickHandler = onClick;
        _selectedRole = defaultRole ?? roles.Random()!.Role;
        RoleCount = availableRoles.Count + 1;
        Coroutines.Start(CoOpen(this));
    }

    private static IEnumerator CoOpen(AmbassadorSelectionMinigame minigame)
    {
        while (ExileController.Instance)
        {
            yield return new WaitForSeconds(0.65f);
        }
        minigame.gameObject.SetActive(true);
        minigame.Begin();
    }

    public override void Close()
    {
        HudManager.Instance.StartCoroutine(HudManager.Instance.CoFadeFullScreen(_bgColor, Color.clear));
        CurrentCard = -1;
        RoleCount = -1;
        MinigameStubs.Close(this);
    }

    private void Begin()
    {
        HudManager.Instance.StartCoroutine(HudManager.Instance.CoFadeFullScreen(Color.clear, _bgColor));

        if (HeaderText != null) StatusText.text = HeaderText;
        if (DefaultRoleNameText != null) RoleName.text = DefaultRoleNameText;
        if (DefaultTeamText != null) RoleTeam.text = DefaultTeamText;

        StatusText.gameObject.SetActive(true);
        RoleName.gameObject.SetActive(true);
        RoleTeam.gameObject.SetActive(true);
        RoleIcon.gameObject.SetActive(true);
        RedRing.SetActive(true);
        WarpRing.SetActive(true);
        RoleIcon.SetSizeLimit(2.8f);

        foreach (var role in availableRoles)
        {
            var teamName = MiscUtils.GetParsedRoleAlignment(role);
            var roleName = role.GetRoleName();
            var roleImg = TouRoleUtils.GetBasicRoleIcon(role);

            if (role is ICustomRole customRole && customRole.Configuration.Icon != null)
            {
                roleImg = customRole.Configuration.Icon.LoadAsset();
            }
            else
            {
                if (role.RoleIconSolid != null)
                {
                    roleImg = role.RoleIconSolid;
                }
            }

            var card = CreateCard(roleName, teamName, roleImg, role.TeamColor);
            card.OnClick.RemoveAllListeners();
            card.OnClick.AddListener((UnityAction)(() => { clickHandler.Invoke(role); }));
        }

        var randomCard = CreateCard(
            RandomCardLabel ?? TouLocale.Get("Random"),
            RandomCardTeamLabel ?? TouLocale.Get("TouRoleAmbassadorRandomImpostorOption"),
            TouRoleIcons.RandomImp.LoadAsset(),
            TownOfUsColors.Impostor);
        randomCard.OnClick.RemoveAllListeners();
        randomCard.OnClick.AddListener((UnityAction)(() =>
        {
            clickHandler.Invoke(RoleManager.Instance.GetRole(_selectedRole!.Value));
        }));

        Coroutines.Start(CoAnimateCards());
        TransType = TransitionType.None;
        Begin(null);
    }

    private PassiveButton CreateCard(string roleName, string teamName, Sprite sprite, Color color)
    {
        var newRoleObj = Instantiate(RolePrefab, RolesHolder);
        var actualCard = newRoleObj!.transform.GetChild(0);
        var roleText = actualCard.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
        var roleImage = actualCard.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        var teamText = actualCard.transform.GetChild(2).gameObject.GetComponent<TextMeshPro>();
        var selection = actualCard.transform.GetChild(3).gameObject;
        var passiveButton = actualCard.GetComponent<PassiveButton>();
        var buttonRollover = actualCard.GetComponent<ButtonRolloverHandler>();

        selection.SetActive(false);
        passiveButton.OnMouseOver.AddListener((UnityAction)(() =>
        {
            selection.SetActive(true);
            RoleName!.text = roleName;
            RoleTeam!.text = teamName;
            if (sprite != null)
            {
                RoleIcon.sprite = sprite;
            }
            RoleIcon.SetSizeLimit(2.8f);
        }));
        passiveButton.OnMouseOut.AddListener((UnityAction)(() => { selection.SetActive(false); }));

        float angle = (2 * Mathf.PI / RoleCount) * CurrentCard;
        float x = 1.9f * Mathf.Cos(angle);
        float y = 0.1f + 1.9f * Mathf.Sin(angle);

        newRoleObj.transform.localPosition = new Vector3(x, y, -1f);
        newRoleObj.name = roleName + " Selection";

        roleText.text = roleName;
        teamText.text = teamName;

        roleImage.sprite = (sprite != null) ? sprite : TouRoleIcons.Impostor.LoadAsset();
        roleImage.SetSizeLimit(2.8f);

        buttonRollover.OverColor = color;
        roleText.color = color;
        teamText.color = color;
        ++CurrentCard;
        newRoleObj.gameObject.SetActive(true);

        return passiveButton;
    }

    [HideFromIl2Cpp]
    private IEnumerator CoAnimateCards()
    {
        foreach (var o in RolesHolder!.transform)
        {
            var card = o.Cast<Transform>();
            if (card == null)
            {
                continue;
            }
            var child = card.GetChild(0);
            Coroutines.Start(MiscUtils.BetterBloop(child, finalSize: 0.5f - (RoleCount * 0.0075f), duration: 0.1f, intensity: 0.11f));
            yield return new WaitForSeconds(0.01f);
        }
        CurrentCard = -1;
        RoleCount = -1;
    }
}

[RegisterInIl2Cpp]
[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Unity")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Unity")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
public sealed class AmbassadorConfirmMinigame(IntPtr cppPtr) : Minigame(cppPtr)
{
    public TextMeshPro TitleText;
    public SpriteRenderer RoleIcon;
    public TextMeshPro RetrainText;
    public GameObject Divider;
    public GameObject Box;
    public GameObject DenyButton;
    public GameObject AcceptButton;
    private RoleBehaviour NewRole;

    private readonly Color _bgColor = new Color32(24, 0, 0, 215);
    private Action<bool> clickHandler;

    public string TitleString = null;
    public string BodyText = null;

    private void Awake()
    {
        if (Instance)
        {
            Instance.Close();
        }

        TitleText = transform.FindChild("Status").FindChild("Title").gameObject.GetComponent<TextMeshPro>();
        RoleIcon = transform.FindChild("Status").FindChild("RoleImage").gameObject.GetComponent<SpriteRenderer>();
        RetrainText = transform.FindChild("Status").FindChild("RetrainText").gameObject.GetComponent<TextMeshPro>();
        Divider = transform.FindChild("Status").FindChild("Divider").gameObject;
        Box = transform.FindChild("Status").FindChild("Box").gameObject;
        DenyButton = transform.FindChild("Status").FindChild("DenyButton").gameObject;
        AcceptButton = transform.FindChild("Status").FindChild("AcceptButton").gameObject;

        TitleText.font = HudManager.Instance.TaskPanel.taskText.font;
        TitleText.fontMaterial = HudManager.Instance.TaskPanel.taskText.fontMaterial;
        TitleText.text = "Ambassador Retrain";

        RetrainText.font = HudManager.Instance.TaskPanel.taskText.font;
        RetrainText.fontMaterial = HudManager.Instance.TaskPanel.taskText.fontMaterial;

        TitleText.gameObject.SetActive(false);
        RoleIcon.gameObject.SetActive(false);
        RetrainText.gameObject.SetActive(false);
        Divider.SetActive(false);
        Box.SetActive(false);
        DenyButton.SetActive(false);
        AcceptButton.SetActive(false);
    }

    public static AmbassadorConfirmMinigame Create()
    {
        var gameObject = Instantiate(TouAssets.ConfirmMinigame.LoadAsset(), HudManager.Instance.transform);
        gameObject.GetComponent<Minigame>().DestroyImmediate();
        gameObject.SetActive(false);
        return gameObject.AddComponent<AmbassadorConfirmMinigame>();
    }

    [HideFromIl2Cpp]
    public void Open(RoleBehaviour role, Action<bool> onClick)
    {
        clickHandler = onClick;
        NewRole = role;
        Coroutines.Start(CoOpen(this));
    }

    private static IEnumerator CoOpen(AmbassadorConfirmMinigame minigame)
    {
        while (ExileController.Instance)
        {
            yield return new WaitForSeconds(0.65f);
        }
        minigame.gameObject.SetActive(true);
        minigame.Begin();
    }

    public override void Close()
    {
        HudManager.Instance.StartCoroutine(HudManager.Instance.CoFadeFullScreen(_bgColor, Color.clear));
        MinigameStubs.Close(this);
    }

    private void Begin()
    {
        HudManager.Instance.StartCoroutine(HudManager.Instance.CoFadeFullScreen(Color.clear, _bgColor));

        if (TitleString != null) TitleText.text = TitleString;
        RetrainText.text = (BodyText ?? $"Are you sure you want to be retrained into {NewRole.GetRoleName()}?\nThis change is permanent.").Replace("{role}", NewRole.GetRoleName());
        RoleIcon.sprite = NewRole.RoleIconWhite ?? TouRoleIcons.Impostor.LoadAsset();
        RoleIcon.SetSizeLimit(2.8f);

        TitleText.gameObject.SetActive(true);
        RoleIcon.gameObject.SetActive(true);
        RetrainText.gameObject.SetActive(true);
        Divider.SetActive(true);
        Box.SetActive(true);
        DenyButton.SetActive(true);
        AcceptButton.SetActive(true);

        DenyButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
        DenyButton.GetComponent<PassiveButton>().OnClick.AddListener((UnityAction)(() =>
        {
            clickHandler.Invoke(false);
        }));

        AcceptButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
        AcceptButton.GetComponent<PassiveButton>().OnClick.AddListener((UnityAction)(() =>
        {
            clickHandler.Invoke(true);
        }));

        TransType = TransitionType.None;
        Begin(null);
    }
}