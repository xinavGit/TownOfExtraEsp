using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using AmongUs.Data;
using AmongUs.Data.Player;
using Assets.InnerNet;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace TownOfExtra;

public class ToExModNews
{
    public ToExModNews(int number, string title, string subTitle, string shortTitle, string text, string date)
    {
        Number = number;
        Title = title;
        SubTitle = subTitle;
        ShortTitle = shortTitle;
        Text = text;
        Date = date;
    }

    public int Number;
    public string Title;
    public string SubTitle;
    public string ShortTitle;
    public string Text;
    public string Date;

    public Announcement ToAnnouncement()
    {
        return new Announcement
        {
            Date = Date,
            Number = Number,
            ShortTitle = ShortTitle,
            SubTitle = SubTitle,
            Title = Title,
            Text = Text,
            Language = (uint)DataManager.Settings.Language.CurrentLanguage,
            Id = "TownOfExtra"
        };
    }
}

public static class ModNewsFetcher
{
    public static void CheckForNews()
    {
        LoadFromResources();
    }

    private static void LoadFromResources()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("TownOfExtra.Resources.News.modNews-en_US.json")
            ?? throw new InvalidOperationException("Embedded news resource not found.");
        using var reader = new StreamReader(stream);
        ParseJson(reader.ReadToEnd());
    }

    private static void ParseJson(string json)
    {
        using var doc = JsonDocument.Parse(json);
        foreach (var item in doc.RootElement.GetProperty("News").EnumerateArray())
        {
            var date = item.GetProperty("Date").GetString() ?? "Unknown Date";
            var numberStr = item.GetProperty("Number").GetString();
            var number = numberStr != null ? int.Parse(numberStr) : 0;
            var shortTitle = item.GetProperty("ShortTitle").GetString() ?? "";
            var subTitle = item.GetProperty("SubTitle").GetString() ?? "";
            var title = item.GetProperty("Title").GetString() ?? "";
            var text = string.Join(" ", item.GetProperty("Text").EnumerateArray().Select(e => e.GetString()));

            ModNewsHistory.AllModNews = ModNewsHistory.AllModNews.Add(
                new ToExModNews(number, title, subTitle, shortTitle, text, date)
            );
        }
    }

    [HarmonyPatch]
    public static class ModNewsHistory
    {
        public static ImmutableList<ToExModNews> AllModNews = ImmutableList<ToExModNews>.Empty;

        [HarmonyPatch(typeof(PlayerAnnouncementData), nameof(PlayerAnnouncementData.SetAnnouncements))]
        [HarmonyPrefix]
        public static void SetAnnouncements_Prefix(ref Il2CppReferenceArray<Announcement> aRange)
        {
            if (AllModNews.Count == 0) return;

            var combined = AllModNews.Select(n => n.ToAnnouncement()).ToList();
            combined.AddRange(aRange.ToArray().Where(a => AllModNews.All(x => x.Number != a.Number)));
            combined.Sort((a, b) => DateTime.Compare(DateTime.Parse(b.Date), DateTime.Parse(a.Date)));

            var result = new Announcement[combined.Count];
            for (var i = 0; i < combined.Count; i++) result[i] = combined[i];
            aRange = result;
        }

        [HarmonyPatch(typeof(AnnouncementPanel), nameof(AnnouncementPanel.SetUp))]
        [HarmonyPostfix]
        public static void SetUpPanel_Postfix(AnnouncementPanel __instance, [HarmonyArgument(0)] Announcement announcement)
        {
            if (announcement.Id != "TownOfExtra") return;

            var obj = new GameObject("ModLabel");
            obj.transform.SetParent(__instance.transform);
            obj.transform.localPosition = new Vector3(-0.8f, 0.13f, 0.5f);
            obj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = TownOfExtraAssets.TownOfExtraIcon.LoadAsset();
            renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }
}