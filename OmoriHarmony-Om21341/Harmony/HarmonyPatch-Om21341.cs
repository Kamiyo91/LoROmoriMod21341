using System.Collections.Generic;
using System.Linq;
using BLL_Om21341.Models;
using HarmonyLib;
using LOR_DiceSystem;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Util_Om21341;

namespace OmoriHarmony_Om21341.Harmony
{
    [HarmonyPatch]
    public class HarmonyPatch_Om21341
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIBookStoryChapterSlot), "SetEpisodeSlots")]
        public static void SetEpisodeSlots(UIBookStoryChapterSlot __instance,
            UIBookStoryPanel ___panel, List<UIBookStoryEpisodeSlot> ___EpisodeSlots)
        {
            SkinUtil.SetEpisodeSlots(__instance, ___panel, ___EpisodeSlots);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BookModel), "GetThumbSprite")]
        [HarmonyPatch(typeof(BookXmlInfo), "GetThumbSprite")]
        public static void GetThumbSprite(object __instance, ref Sprite __result)
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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIBattleSettingPanel), "SetToggles")]
        public static void SetToggles(UIBattleSettingPanel __instance)
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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BookModel), "SetXmlInfo")]
        public static void SetXmlInfo(BookModel __instance, ref List<DiceCardXmlInfo> ____onlyCards)
        {
            if (__instance.BookId.packageId != ModParameters.PackageId) return;
            var onlyCards = ModParameters.OnlyCardKeywords.Where(x => x.Item3 == __instance.BookId.id)
                .Select(x => x.Item2).ToList();
            if (!onlyCards.Any()) return;
            foreach (var onlyCard in onlyCards)
                ____onlyCards.AddRange(onlyCard.Select(id =>
                    ItemXmlDataList.instance.GetCardItem(new LorId(ModParameters.PackageId, id))));
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StageLibraryFloorModel), "InitUnitList")]
        public static void InitUnitList(StageLibraryFloorModel __instance,
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

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnitDataModel), "EquipBook")]
        public static void EquipBookPrefix(UnitDataModel __instance, bool force)
        {
            if (force) return;
            if (ModParameters.PackageId == __instance.bookItem.ClassInfo.id.packageId &&
                ModParameters.DynamicNames.ContainsKey(__instance.bookItem.ClassInfo.id.id)) __instance.ResetTempName();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UnitDataModel), "EquipBook")]
        public static void EquipBookPostfix(UnitDataModel __instance, BookModel newBook, bool force)
        {
            if (force) return;
            if (newBook == null || ModParameters.PackageId != newBook.ClassInfo.workshopID ||
                !ModParameters.DynamicNames.ContainsKey(newBook.ClassInfo.id.id)) return;
            if (UnitUtil.CheckSkinUnitData(__instance)) return;
            var nameId = ModParameters.DynamicNames[newBook.ClassInfo.id.id].ToString();
            __instance.SetTempName(ModParameters.NameTexts[nameId]);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BookModel), "CanSuccessionPassive")]
        public static void BookModel_CanSuccessionPassive(BookModel __instance, PassiveModel targetpassive,
            ref GivePassiveState haspassiveState, ref bool __result)
        {
            var passiveItemExtra =
                ModParameters.ExtraConditionPassives.FirstOrDefault(x =>
                    x.Item1 == targetpassive.originData.currentpassive.id);
            if (passiveItemExtra == null || !__instance.GetPassiveModelList()
                    .Exists(x => passiveItemExtra.Item2 == x.reservedData.currentpassive.id)) return;
            haspassiveState = GivePassiveState.Lock;
            __result = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UnitDataModel), "LoadFromSaveData")]
        public static void Unit_LoadFromSaveData(UnitDataModel __instance)
        {
            if ((!string.IsNullOrEmpty(__instance.workshopSkin) || __instance.bookItem != __instance.CustomBookItem) &&
                __instance.bookItem.ClassInfo.id.packageId == ModParameters.PackageId &&
                ModParameters.DynamicNames.ContainsKey(__instance.bookItem.ClassInfo.id.id))
                __instance.ResetTempName();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UICustomizePopup), "OnClickSave")]
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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TextDataModel), "InitTextData")]
        public static void TextDataModel_InitTextData(string currentLanguage)
        {
            ModParameters.Language = currentLanguage;
            LocalizeUtil.AddLocalize();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UISettingInvenEquipPageListSlot), "SetBooksData")]
        [HarmonyPatch(typeof(UIInvenEquipPageListSlot), "SetBooksData")]
        public static void SetBooksData(object __instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            var uiOrigin = __instance as UIOriginEquipPageList;
            SkinUtil.SetBooksData(uiOrigin, books, storyKey);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UISpriteDataManager), "Init")]
        public static void Init(UISpriteDataManager __instance)
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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StageController), "BonusRewardWithPopup")]
        public static void BonusRewardWithPopup(LorId stageId)
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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(DropBookInventoryModel), "LoadFromSaveData")]
        public static void Book_LoadFromSaveData(DropBookInventoryModel __instance)
        {
            var bookCount = __instance.GetBookCount(new LorId(ModParameters.PackageId, 10));
            if (bookCount < 99) __instance.AddBook(new LorId(ModParameters.PackageId, 10), 99 - bookCount);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UICharacterListPanel), "RefreshBattleUnitDataModel")]
        public static void RefreshBattleUnitDataModel(UICharacterListPanel __instance,
            UnitDataModel data)
        {
            if (Singleton<StageController>.Instance.GetStageModel() == null ||
                Singleton<StageController>.Instance.GetStageModel().ClassInfo.id.packageId != ModParameters.PackageId ||
                !ModParameters.UniqueUnitStages.ContainsKey(Singleton<StageController>.Instance.GetStageModel()
                    .ClassInfo.id.id)) return;
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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIBookStoryPanel), "OnSelectEpisodeSlot")]
        public static void UIBookStoryPanel_OnSelectEpisodeSlot(UIBookStoryPanel __instance,
            UIBookStoryEpisodeSlot slot, TextMeshProUGUI ___selectedEpisodeText, Image ___selectedEpisodeIcon,
            Image ___selectedEpisodeIconGlow)
        {
            if (slot == null || slot.books.Find(x => x.id.packageId == ModParameters.PackageId) == null) return;
            ___selectedEpisodeText.text = ModParameters.EffectTexts
                .FirstOrDefault(x => x.Key.Equals("CredenzaName_Om21341")).Value
                .Name;
            ___selectedEpisodeIcon.sprite = ModParameters.ArtWorks["Omori_Om21341"];
            ___selectedEpisodeIconGlow.sprite = ModParameters.ArtWorks["Omori_Om21341"];
            __instance.UpdateBookSlots();
        }
    }
}