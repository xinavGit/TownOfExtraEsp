using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TownOfExtra;

public static class TownOfExtraAssets
{
    public static LoadableAsset<Sprite> Placeholder =>
        new LoadableResourceAsset("TownOfExtra.Resources.Placeholder.png");
    
    public static LoadableAsset<Sprite> PoisonerRoleIcon =>
        new LoadableResourceAsset("TownOfExtra.Resources.PoisonerRoleIcon.png");
    public static LoadableAsset<Sprite> FreezerRoleIcon =>
        new LoadableResourceAsset("TownOfExtra.Resources.FreezerRoleIcon.png");
    public static LoadableAsset<Sprite> CannibalRoleIcon =>
        new LoadableResourceAsset("TownOfExtra.Resources.CannibalRoleIcon.png");
    public static LoadableAsset<Sprite> TricksterRoleIcon =>
        new LoadableResourceAsset("TownOfExtra.Resources.TricksterRoleIcon.png");
    public static LoadableAsset<Sprite> HeavyWorkloadModifierIcon =>
        new LoadableResourceAsset("TownOfExtra.Resources.HeavyWorkloadModifierIcon.png");
    
    public static LoadableAsset<Sprite> CannibalEatButton =>
        new LoadableResourceAsset("TownOfExtra.Resources.CannibalEatButton.png");
    public static LoadableAsset<Sprite> PoisonerPoisonButton =>
        new LoadableResourceAsset("TownOfExtra.Resources.PoisonerPoisonButton.png");
    public static LoadableAsset<Sprite> FreezerFreezeButton =>
        new LoadableResourceAsset("TownOfExtra.Resources.FreezerFreezeButton.png");
}