using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BLL_Om21341.Models;
using BLL_Om21341.Models.Enum;
using HarmonyLib;
using LOR_DiceSystem;
using LOR_XML;
using TMPro;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util_Om21341
{
    public static class UnitUtil
    {
        public static bool CantUseCardAfraid(BattleDiceCardModel card)
        {
            return card.XmlData.Spec.Ranged == CardRange.FarArea ||
                   card.XmlData.Spec.Ranged == CardRange.FarAreaEach || card.GetOriginCost() > 3 ||
                   card.XmlData.IsEgo() || card.XmlData.id.packageId == ModParameters.PackageId &&
                   (card.XmlData.id.id == 32 || card.XmlData.id.id == 33 || card.XmlData.id.id == 34 ||
                    card.XmlData.id.id == 35);
        }

        public static bool CheckSkinProjection(BattleUnitModel owner)
        {
            if (!string.IsNullOrEmpty(owner.UnitData.unitData.workshopSkin)) return true;
            if (owner.UnitData.unitData.bookItem == owner.UnitData.unitData.CustomBookItem) return false;
            owner.view.ChangeSkin(owner.UnitData.unitData.CustomBookItem.GetCharacterName());
            return true;
        }

        public static void RefreshCombatUI(bool forceReturn = false)
        {
            foreach (var (battleUnit, num) in BattleObjectManager.instance.GetList()
                         .Select((value, i) => (value, i)))
            {
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnit.UnitData.unitData, num, true);
                if (forceReturn)
                    battleUnit.moveDetail.ReturnToFormationByBlink(true);
            }

            BattleObjectManager.instance.InitUI();
        }

        public static void UnitReviveAndRecovery(BattleUnitModel owner, int hp, bool recoverLight,
            bool skinChanged = false)
        {
            if (owner.IsDead())
            {
                owner.bufListDetail.GetActivatedBufList()
                    .RemoveAll(x => !x.CanRecoverHp(999) || !x.CanRecoverBreak(999));
                owner.Revive(hp);
                owner.moveDetail.ReturnToFormationByBlink(true);
                owner.view.EnableView(true);
                if (skinChanged)
                    CheckSkinProjection(owner);
                else
                    owner.view.CreateSkin();
            }
            else
            {
                owner.bufListDetail.GetActivatedBufList()
                    .RemoveAll(x => !x.CanRecoverHp(999) || !x.CanRecoverBreak(999));
                owner.RecoverHP(hp);
            }

            owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
            owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_sealTemp));
            owner.breakDetail.ResetGauge();
            owner.breakDetail.nextTurnBreak = false;
            owner.breakDetail.RecoverBreakLife(1, true);
            if (recoverLight) owner.cardSlotDetail.RecoverPlayPoint(owner.cardSlotDetail.GetMaxPlayPoint());
        }

        public static void ReadyCounterCard(BattleUnitModel owner, int id)
        {
            var card = BattleDiceCardModel.CreatePlayingCard(
                ItemXmlDataList.instance.GetCardItem(new LorId(ModParameters.PackageId, id)));
            owner.cardSlotDetail.keepCard.AddBehaviours(card, card.CreateDiceCardBehaviorList());
            owner.allyCardDetail.ExhaustCardInHand(card);
        }

        public static void SetPassiveCombatLog(PassiveAbilityBase passive, BattleUnitModel owner)
        {
            var battleCardResultLog = owner.battleCardResultLog;
            battleCardResultLog?.SetPassiveAbility(passive);
        }

        public static void Add4SephirahUnits(StageModel stage,
            List<UnitBattleDataModel> unitList)
        {
            var units = new List<UnitBattleDataModel>
            {
                InitUnitDefault(stage, LibraryModel.Instance.GetOpenedFloorList()
                    .FirstOrDefault(x => x.Sephirah == SephirahType.Malkuth)
                    ?.GetUnitDataList().FirstOrDefault(y => y.isSephirah)),
                InitUnitDefault(stage, LibraryModel.Instance.GetOpenedFloorList()
                    .FirstOrDefault(x => x.Sephirah == SephirahType.Yesod)
                    ?.GetUnitDataList().FirstOrDefault(y => y.isSephirah)),
                InitUnitDefault(stage, LibraryModel.Instance.GetOpenedFloorList()
                    .FirstOrDefault(x => x.Sephirah == SephirahType.Hod)
                    ?.GetUnitDataList().FirstOrDefault(y => y.isSephirah)),
                InitUnitDefault(stage, LibraryModel.Instance.GetOpenedFloorList()
                    .FirstOrDefault(x => x.Sephirah == SephirahType.Netzach)
                    ?.GetUnitDataList().FirstOrDefault(y => y.isSephirah))
            };
            unitList?.AddRange(units);
        }

        private static UnitBattleDataModel InitUnitDefault(StageModel stage, UnitDataModel data)
        {
            var unitBattleDataModel = new UnitBattleDataModel(stage, data);
            unitBattleDataModel.Init();
            return unitBattleDataModel;
        }

        public static List<UnitBattleDataModel> UnitsToRecover(StageModel stageModel, UnitDataModel data)
        {
            var list = new List<UnitBattleDataModel>();
            foreach (var sephirah in ModParameters.UniqueUnitStages
                         .FirstOrDefault(x => x.Key.Equals(stageModel.ClassInfo.id.id)).Value)
                list.AddRange(stageModel.GetFloor(sephirah).GetUnitBattleDataList()
                    .Where(x => x.unitData == data));
            return list;
        }

        public static void BattleAbDialog(BattleDialogUI instance, List<AbnormalityCardDialog> dialogs,
            AbColorType colorType)
        {
            var component = instance.GetComponent<CanvasGroup>();
            var dialog = dialogs[Random.Range(0, dialogs.Count)].dialog;
            var txtAbnormalityDlg = (TextMeshProUGUI)typeof(BattleDialogUI).GetField("_txtAbnormalityDlg",
                AccessTools.all)?.GetValue(instance);
            txtAbnormalityDlg.text = dialog;
            if (colorType == AbColorType.Positive)
            {
                txtAbnormalityDlg.fontMaterial.SetColor("_GlowColor",
                    SingletonBehavior<BattleManagerUI>.Instance.positiveCoinColor);
                txtAbnormalityDlg.color = SingletonBehavior<BattleManagerUI>.Instance.positiveTextColor;
            }
            else
            {
                txtAbnormalityDlg.fontMaterial.SetColor("_GlowColor",
                    SingletonBehavior<BattleManagerUI>.Instance.negativeCoinColor);
                txtAbnormalityDlg.color = SingletonBehavior<BattleManagerUI>.Instance.negativeTextColor;
            }

            var canvas = (Canvas)typeof(BattleDialogUI).GetField("_canvas",
                AccessTools.all)?.GetValue(instance);
            canvas.enabled = true;
            component.interactable = true;
            component.blocksRaycasts = true;
            txtAbnormalityDlg.GetComponent<AbnormalityDlgEffect>().Init();
            var _ = (Coroutine)typeof(BattleDialogUI).GetField("_routine",
                AccessTools.all)?.GetValue(instance);
            var method = typeof(BattleDialogUI).GetMethod("AbnormalityDlgRoutine", AccessTools.all);
            instance.StartCoroutine(method.Invoke(instance, Array.Empty<object>()) as IEnumerator);
        }

        private static void SetBaseKeywordCard(LorId id, ref Dictionary<LorId, DiceCardXmlInfo> cardDictionary,
            ref List<DiceCardXmlInfo> cardXmlList)
        {
            var keywordsList = GetKeywordsList(id.id).ToList();
            var diceCardXmlInfo2 = CardOptionChange(cardDictionary[id], new List<CardOption>(), true, keywordsList);
            cardDictionary[id] = diceCardXmlInfo2;
            cardXmlList.Add(diceCardXmlInfo2);
        }

        private static void SetCustomCardOption(CardOption option, LorId id, bool keywordsRequired,
            ref Dictionary<LorId, DiceCardXmlInfo> cardDictionary, ref List<DiceCardXmlInfo> cardXmlList)
        {
            var keywordsList = new List<string>();
            if (keywordsRequired) keywordsList = GetKeywordsList(id.id).ToList();
            var diceCardXmlInfo2 = CardOptionChange(cardDictionary[id], new List<CardOption> { option },
                keywordsRequired,
                keywordsList);
            cardDictionary[id] = diceCardXmlInfo2;
            cardXmlList.Add(diceCardXmlInfo2);
        }

        private static List<int> GetAllOnlyCardsId()
        {
            var onlyPageCardList = new List<int>();
            foreach (var cardIds in ModParameters.OnlyCardKeywords.Select(x => x.Item2))
                onlyPageCardList.AddRange(cardIds);
            return onlyPageCardList;
        }

        private static IEnumerable<string> GetKeywordsList(int id)
        {
            var keyword = ModParameters.OnlyCardKeywords.FirstOrDefault(x => x.Item2.Contains(id))?.Item1;
            return string.IsNullOrEmpty(keyword)
                ? new List<string> { "OmoriModPage_Om21341" }
                : new List<string> { "OmoriModPage_Om21341", keyword };
        }

        private static DiceCardXmlInfo CardOptionChange(DiceCardXmlInfo cardXml, List<CardOption> option,
            bool keywordRequired, List<string> keywords,
            string skinName = "", string mapName = "", int skinHeight = 0)
        {
            return new DiceCardXmlInfo(cardXml.id)
            {
                workshopID = cardXml.workshopID,
                workshopName = cardXml.workshopName,
                Artwork = cardXml.Artwork,
                Chapter = cardXml.Chapter,
                category = cardXml.category,
                DiceBehaviourList = cardXml.DiceBehaviourList,
                _textId = cardXml._textId,
                optionList = option.Any() ? option : cardXml.optionList,
                Priority = cardXml.Priority,
                Rarity = cardXml.Rarity,
                Script = cardXml.Script,
                ScriptDesc = cardXml.ScriptDesc,
                Spec = cardXml.Spec,
                SpecialEffect = cardXml.SpecialEffect,
                SkinChange = string.IsNullOrEmpty(skinName) ? cardXml.SkinChange : skinName,
                SkinChangeType = cardXml.SkinChangeType,
                SkinHeight = skinHeight != 0 ? skinHeight : cardXml.SkinHeight,
                MapChange = string.IsNullOrEmpty(mapName) ? cardXml.MapChange : mapName,
                PriorityScript = cardXml.PriorityScript,
                Keywords = keywordRequired ? keywords : cardXml.Keywords
            };
        }

        public static void ChangeCardItem(ItemXmlDataList instance)
        {
            var dictionary = (Dictionary<LorId, DiceCardXmlInfo>)instance.GetType()
                .GetField("_cardInfoTable", AccessTools.all).GetValue(instance);
            var list = (List<DiceCardXmlInfo>)instance.GetType()
                .GetField("_cardInfoList", AccessTools.all).GetValue(instance);
            var onlyPageCardList = GetAllOnlyCardsId();
            foreach (var (key, _) in dictionary.Where(x => x.Key.packageId == ModParameters.PackageId).ToList())
            {
                if (ModParameters.PersonalCardList.Contains(key.id))
                {
                    SetCustomCardOption(CardOption.Personal, key, false, ref dictionary, ref list);
                    continue;
                }

                if (ModParameters.EgoPersonalCardList.Contains(key.id))
                {
                    SetCustomCardOption(CardOption.EgoPersonal, key, false, ref dictionary, ref list);
                    continue;
                }

                if (onlyPageCardList.Contains(key.id))
                {
                    SetCustomCardOption(CardOption.OnlyPage, key, true, ref dictionary, ref list);
                    continue;
                }

                SetBaseKeywordCard(key, ref dictionary, ref list);
            }
        }

        public static void ChangePassiveItem()
        {
            foreach (var passive in Singleton<PassiveXmlList>.Instance.GetDataAll().Where(passive =>
                         passive.id.packageId == ModParameters.PackageId &&
                         ModParameters.UntransferablePassives.Contains(passive.id.id)))
                passive.CanGivePassive = false;
            foreach (var (key, value) in ModParameters.SameInnerIdPassives)
            foreach (var passive in Singleton<PassiveXmlList>.Instance.GetDataAll().Where(passive =>
                         passive.id.packageId == ModParameters.PackageId &&
                         value.Contains(passive.id.id)))
                passive.InnerTypeId = key;
        }
    }
}