using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BLL_Om21341.Models;
using HarmonyLib;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Util_Om21341;

namespace OmoriHarmony_Om21341.Harmony
{
    public class OmoriHarmony_Om21341 : ModInitializer
    {
        public override void OnInitializeMod()
        {
            ModParameters.Path = Path.GetDirectoryName(
                Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            var harmony = new HarmonyLib.Harmony("LOR.OmoriModOm21341_MOD");
            var method = typeof(OmoriHarmony_Om21341).GetMethod("BookModel_SetXmlInfo");
            harmony.Patch(typeof(BookModel).GetMethod("SetXmlInfo", AccessTools.all), null, new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("General_GetThumbSprite");
            harmony.Patch(typeof(BookModel).GetMethod("GetThumbSprite", AccessTools.all), null,
                new HarmonyMethod(method));
            harmony.Patch(typeof(BookXmlInfo).GetMethod("GetThumbSprite", AccessTools.all), null,
                new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("StageLibraryFloorModel_InitUnitList");
            harmony.Patch(typeof(StageLibraryFloorModel).GetMethod("InitUnitList", AccessTools.all),
                null, new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("General_SetBooksData");
            harmony.Patch(typeof(UISettingInvenEquipPageListSlot).GetMethod("SetBooksData", AccessTools.all),
                null, new HarmonyMethod(method));
            harmony.Patch(typeof(UIInvenEquipPageListSlot).GetMethod("SetBooksData", AccessTools.all),
                null, new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("UISpriteDataManager_Init");
            harmony.Patch(typeof(UISpriteDataManager).GetMethod("Init", AccessTools.all),
                new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("UIBookStoryChapterSlot_SetEpisodeSlots");
            harmony.Patch(typeof(UIBookStoryChapterSlot).GetMethod("SetEpisodeSlots", AccessTools.all),
                null, new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("UIBattleSettingPanel_SetToggles");
            harmony.Patch(typeof(UIBattleSettingPanel).GetMethod("SetToggles", AccessTools.all),
                null, new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("UnitDataModel_EquipBookPrefix");
            var methodPostfix = typeof(OmoriHarmony_Om21341).GetMethod("UnitDataModel_EquipBookPostfix");
            harmony.Patch(typeof(UnitDataModel).GetMethod("EquipBook", AccessTools.all),
                new HarmonyMethod(method), new HarmonyMethod(methodPostfix));
            method = typeof(OmoriHarmony_Om21341).GetMethod("UnitDataModel_LoadFromSaveData");
            harmony.Patch(typeof(UnitDataModel).GetMethod("LoadFromSaveData", AccessTools.all),
                null, new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("UICustomizePopup_OnClickSave");
            harmony.Patch(typeof(UICustomizePopup).GetMethod("OnClickSave", AccessTools.all),
                new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("TextDataModel_InitTextData");
            harmony.Patch(typeof(TextDataModel).GetMethod("InitTextData", AccessTools.all),
                null, new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("StageController_BonusRewardWithPopup");
            harmony.Patch(typeof(StageController).GetMethod("BonusRewardWithPopup", AccessTools.all),
                null, new HarmonyMethod(method));
            method = typeof(OmoriHarmony_Om21341).GetMethod("UICharacterListPanel_RefreshBattleUnitDataModel");
            harmony.Patch(typeof(UICharacterListPanel).GetMethod("RefreshBattleUnitDataModel", AccessTools.all),
                null, new HarmonyMethod(method));
            ModParameters.Language = GlobalGameManager.Instance.CurrentOption.language;
            MapUtil.GetArtWorks(new DirectoryInfo(ModParameters.Path + "/ArtWork"));
            UnitUtil.AddBookOnGameStart(Singleton<DropBookInventoryModel>.Instance);
            UnitUtil.ChangeCardItem(ItemXmlDataList.instance);
            UnitUtil.ChangePassiveItem();
            SkinUtil.PreLoadBufIcons();
            LocalizeUtil.AddLocalize();
            LocalizeUtil.RemoveError();
        }

        public static void UIBookStoryChapterSlot_SetEpisodeSlots(UIBookStoryChapterSlot __instance,
            List<UIBookStoryEpisodeSlot> ___EpisodeSlots)
        {
            SkinUtil.SetEpisodeSlots(__instance, ___EpisodeSlots);
        }

        public static void General_GetThumbSprite(object __instance, ref Sprite __result)
        {
            switch (__instance)
            {
                case BookXmlInfo bookInfo:
                    SkinUtil.GetThumbSprite(bookInfo.id, ref __result);
                    break;
                case BookModel bookModel:
                    SkinUtil.GetThumbSprite(bookModel.BookId, ref __result);
                    break;
            }
        }

        public static void UIBattleSettingPanel_SetToggles(UIBattleSettingPanel __instance)
        {
            if (!Singleton<StageController>.Instance.GetStageModel().ClassInfo.id.packageId
                    .Contains(ModParameters.PackageId)) return;
            if (ModParameters.PreBattleUnits == Singleton<StageController>.Instance.GetStageModel().ClassInfo
                    .id.id) return;
            foreach (var currentAvailbleUnitslot in __instance.currentAvailbleUnitslots)
            {
                currentAvailbleUnitslot.SetToggle(false);
                currentAvailbleUnitslot.SetYesToggleState();
            }

            __instance.SetAvailibleText();
        }

        public static void BookModel_SetXmlInfo(BookModel __instance, ref List<DiceCardXmlInfo> ____onlyCards)
        {
            if (__instance.BookId.packageId != ModParameters.PackageId) return;
            var onlyCards = ModParameters.OnlyCardKeywords.FirstOrDefault(x => x.Item3 == __instance.BookId.id)?.Item2;
            if (onlyCards != null)
                ____onlyCards.AddRange(onlyCards.Select(id =>
                    ItemXmlDataList.instance.GetCardItem(new LorId(ModParameters.PackageId, id))));
        }

        public static void StageLibraryFloorModel_InitUnitList(StageLibraryFloorModel __instance,
            List<UnitBattleDataModel> ____unitList, StageModel stage)
        {
            if (stage.ClassInfo.id.packageId != ModParameters.PackageId) return;
            switch (stage.ClassInfo.id.id)
            {
                case 8:
                    if (__instance.Sephirah == SephirahType.Malkuth ||
                        __instance.Sephirah == SephirahType.Hod ||
                        __instance.Sephirah == SephirahType.Yesod ||
                        __instance.Sephirah == SephirahType.Netzach) ____unitList.Clear();
                    UnitUtil.Add4SephirahUnits(stage, ____unitList);
                    return;
            }
        }

        public static void UnitDataModel_EquipBookPrefix(UnitDataModel __instance, BookModel newBook, bool force)
        {
            if (force) return;
            if (ModParameters.PackageId == __instance.bookItem.ClassInfo.id.packageId &&
                ModParameters.DynamicNames.ContainsKey(__instance.bookItem.ClassInfo.id.id)) __instance.ResetTempName();
        }

        public static void UnitDataModel_EquipBookPostfix(UnitDataModel __instance, BookModel newBook, bool force)
        {
            if (force) return;
            if (newBook == null || ModParameters.PackageId != newBook.ClassInfo.workshopID ||
                !ModParameters.DynamicNames.ContainsKey(newBook.ClassInfo.id.id)) return;
            __instance.EquipCustomCoreBook(null);
            __instance.workshopSkin = "";
            var nameId = ModParameters.DynamicNames[newBook.ClassInfo.id.id].ToString();
            __instance.SetTempName(ModParameters.NameTexts[nameId]);
        }

        public static void UnitDataModel_LoadFromSaveData(UnitDataModel __instance)
        {
            if ((!string.IsNullOrEmpty(__instance.workshopSkin) || __instance.bookItem != __instance.CustomBookItem) &&
                __instance.bookItem.ClassInfo.id.packageId == ModParameters.PackageId &&
                ModParameters.DynamicNames.ContainsKey(__instance.bookItem.ClassInfo.id.id))
                __instance.ResetTempName();
        }

        public static void UICustomizePopup_OnClickSave(UICustomizePopup __instance)
        {
            if (__instance.SelectedUnit.bookItem.ClassInfo.id.packageId != ModParameters.PackageId ||
                !ModParameters.DynamicNames.ContainsKey(__instance.SelectedUnit.bookItem.ClassInfo.id.id)) return;
            var tempName =
                (string)__instance.SelectedUnit.GetType().GetField("_tempName", AccessTools.all)
                    ?.GetValue(__instance.SelectedUnit);
            __instance.SelectedUnit.ResetTempName();
            if (__instance.SelectedUnit.bookItem == __instance.SelectedUnit.CustomBookItem &&
                string.IsNullOrEmpty(__instance.SelectedUnit.workshopSkin))
            {
                __instance.previewData.Name = __instance.SelectedUnit.name;
                var nameId = ModParameters.DynamicNames[__instance.SelectedUnit.bookItem.ClassInfo.id.id].ToString();
                __instance.SelectedUnit.SetTempName(ModParameters.NameTexts[nameId]);
            }
            else
            {
                if (string.IsNullOrEmpty(tempName) || __instance.previewData.Name == tempName)
                    __instance.previewData.Name = __instance.SelectedUnit.name;
            }
        }

        public static void TextDataModel_InitTextData(string currentLanguage)
        {
            ModParameters.Language = currentLanguage;
            LocalizeUtil.AddLocalize();
        }

        public static void General_SetBooksData(object __instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            var uiOrigin = __instance as UIOriginEquipPageList;
            SkinUtil.SetBooksData(uiOrigin, books, storyKey);
        }

        public static void UISpriteDataManager_Init(UISpriteDataManager __instance)
        {
            foreach (var artWork in ModParameters.ArtWorks.Where(x =>
                         !x.Key.Contains("Glow") && !__instance._storyicons.Exists(y => y.type.Equals(x.Key))))
                __instance._storyicons.Add(new UIIconManager.IconSet
                {
                    type = artWork.Key,
                    icon = artWork.Value,
                    iconGlow = ModParameters.ArtWorks.FirstOrDefault(x => x.Key.Equals($"{artWork.Key}Glow")).Value ??
                               artWork.Value
                });
        }

        public static void StageController_BonusRewardWithPopup(LorId stageId)
        {
            if (stageId.packageId != ModParameters.PackageId) return;
            if (!ModParameters.ExtraReward.ContainsKey(stageId.id)) return;
            var parameters = ModParameters.ExtraReward.FirstOrDefault(y => y.Key.Equals(stageId.id));
            foreach (var book in parameters.Value.DroppedBooks)
                Singleton<DropBookInventoryModel>.Instance.AddBook(new LorId(ModParameters.PackageId, book.BookId),
                    book.Quantity);
            UIAlarmPopup.instance.SetAlarmText(ModParameters.EffectTexts.FirstOrDefault(x =>
                    x.Key.Contains(parameters.Value.MessageId))
                .Value
                .Desc);
        }

        public static void UICharacterListPanel_RefreshBattleUnitDataModel(UICharacterListPanel __instance,
            UnitDataModel data)
        {
            if (Singleton<StageController>.Instance.GetStageModel() == null ||
                Singleton<StageController>.Instance.GetStageModel().ClassInfo.id.packageId != ModParameters.PackageId ||
                Singleton<StageController>.Instance.GetStageModel().ClassInfo.id.id != 8) return;
            var slot =
                typeof(UICharacterListPanel).GetField("CharacterList", AccessTools.all)?.GetValue(__instance) as
                    UICharacterList;
            var stageModel = Singleton<StageController>.Instance.GetStageModel();
            var list = UnitUtil.UnitsToRecover(stageModel, data);
            foreach (var unit in list)
            {
                unit.Refreshhp();
                var uicharacterSlot = slot?.slotList.Find(x => x.unitBattleData == unit);
                if (uicharacterSlot == null || uicharacterSlot.unitBattleData == null) continue;
                uicharacterSlot.ReloadHpBattleSettingSlot();
            }
        }
    }
}