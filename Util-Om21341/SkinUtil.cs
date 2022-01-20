using System.Collections.Generic;
using System.Linq;
using BLL_Om21341.Models;
using HarmonyLib;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Util_Om21341
{
    public static class SkinUtil
    {
        public static void SetBooksData(UIOriginEquipPageList instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            if (storyKey.workshopId != ModParameters.PackageId) return;
            var image = (Image)instance.GetType().GetField("img_IconGlow", AccessTools.all).GetValue(instance);
            var image2 = (Image)instance.GetType().GetField("img_Icon", AccessTools.all).GetValue(instance);
            var textMeshProUGUI = (TextMeshProUGUI)instance.GetType().GetField("txt_StoryName", AccessTools.all)
                .GetValue(instance);
            if (books.Count < 0) return;
            image.enabled = true;
            image2.enabled = true;
            image2.sprite = ModParameters.ArtWorks["Omori_Om21341"];
            image.sprite = ModParameters.ArtWorks["Omori_Om21341"];
            textMeshProUGUI.text = ModParameters.EffectTexts.FirstOrDefault(x => x.Key.Equals("CredenzaName_Om21341"))
                .Value
                .Name;
        }

        public static void SetEpisodeSlots(UIBookStoryChapterSlot instance, UIBookStoryPanel panel,
            List<UIBookStoryEpisodeSlot> episodeSlots)
        {
            if (instance.chapter != 7) return;
            var uibookStoryEpisodeSlot =
                episodeSlots.Find(x => x.books.Find(y => y.id.packageId == ModParameters.PackageId) != null);
            if (uibookStoryEpisodeSlot == null) return;
            var books = uibookStoryEpisodeSlot.books;
            uibookStoryEpisodeSlot.Init(
                panel.panel.GetChapterBooksData(instance.chapter).FindAll(x =>
                    x.id.packageId == ModParameters.PackageId && ModParameters.BooksIds.Contains(x.id.id)), instance);
            ((TextMeshProUGUI)uibookStoryEpisodeSlot.GetType().GetField("episodeText", AccessTools.all)
                .GetValue(uibookStoryEpisodeSlot)).text = ModParameters.EffectTexts
                .FirstOrDefault(x => x.Key.Equals("CredenzaName_Om21341")).Value
                .Name;
            var image = (Image)uibookStoryEpisodeSlot.GetType().GetField("episodeIconGlow", AccessTools.all)
                .GetValue(uibookStoryEpisodeSlot);
            var image2 = (Image)uibookStoryEpisodeSlot.GetType().GetField("episodeIcon", AccessTools.all)
                .GetValue(uibookStoryEpisodeSlot);
            image2.sprite = ModParameters.ArtWorks["Omori_Om21341"];
            image.sprite = ModParameters.ArtWorks["Omori_Om21341"];
            instance.InstatiateAdditionalSlot();
            var uibookStoryEpisodeSlot2 = episodeSlots[episodeSlots.Count - 1];
            books.RemoveAll(x => x.id.packageId == ModParameters.PackageId);
            uibookStoryEpisodeSlot2.Init(instance.chapter, books, instance);
        }

        public static void GetThumbSprite(LorId bookId, ref Sprite result)
        {
            if (bookId.packageId != ModParameters.PackageId) return;
            var sprite = ModParameters.SpritePreviewChange.FirstOrDefault(x => x.Value.Contains(bookId.id));
            if (string.IsNullOrEmpty(sprite.Key) || !sprite.Value.Any()) return;
            result = ModParameters.ArtWorks[sprite.Key];
        }

        public static void PreLoadBufIcons()
        {
            foreach (var baseGameIcon in Resources.LoadAll<Sprite>("Sprites/BufIconSheet/")
                         .Where(x => !BattleUnitBuf._bufIconDictionary.ContainsKey(x.name)))
                BattleUnitBuf._bufIconDictionary.Add(baseGameIcon.name, baseGameIcon);
            foreach (var artWork in ModParameters.ArtWorks.Where(x =>
                         !x.Key.Contains("Glow") && !x.Key.Contains("Default") &&
                         !BattleUnitBuf._bufIconDictionary.ContainsKey(x.Key)))
                BattleUnitBuf._bufIconDictionary.Add(artWork.Key, artWork.Value);
        }
    }
}