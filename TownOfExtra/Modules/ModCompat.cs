#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BepInEx.Unity.IL2CPP;

namespace TownOfExtra.Modules;

public static class ModCompat
{
    public const string AApiId = "AchievementsAPI";
    
    public static bool IsLoaded(string modId, [NotNullWhen(true)] out Assembly? assembly)
    {
        var result = IL2CPPChainloader.Instance.Plugins.TryGetValue(modId, out var plugin);
        assembly = result ? plugin?.Instance.GetType().Assembly : null;
        return result;
    }
}