using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BLL_Om21341.Models;
using HarmonyLib;
using LOR_XML;
using Mod;

namespace Util_Om21341
{
    public static class LocalizeUtil
    {
        public static void AddLocalize()
        {
            var dictionary =
                typeof(BattleEffectTextsXmlList).GetField("_dictionary", AccessTools.all)
                    ?.GetValue(Singleton<BattleEffectTextsXmlList>.Instance) as Dictionary<string, BattleEffectText>;
            var files = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language + "/EffectTexts")
                .GetFiles();
            ModParameters.EffectTexts.Clear();
            foreach (var t in files)
                using (var stringReader = new StringReader(File.ReadAllText(t.FullName)))
                {
                    var battleEffectTextRoot =
                        (BattleEffectTextRoot)new XmlSerializer(typeof(BattleEffectTextRoot))
                            .Deserialize(stringReader);
                    foreach (var battleEffectText in battleEffectTextRoot.effectTextList)
                    {
                        dictionary.Remove(battleEffectText.ID);
                        dictionary?.Add(battleEffectText.ID, battleEffectText);
                        ModParameters.EffectTexts.Add(battleEffectText.ID,
                            new EffectTextModel { Name = battleEffectText.Name, Desc = battleEffectText.Desc });
                    }
                }

            files = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language + "/BattlesCards")
                .GetFiles();
            foreach (var t in files)
                using (var stringReader2 = new StringReader(File.ReadAllText(t.FullName)))
                {
                    var battleCardDescRoot =
                        (BattleCardDescRoot)new XmlSerializer(typeof(BattleCardDescRoot)).Deserialize(
                            stringReader2);
                    using (var enumerator =
                           ItemXmlDataList.instance.GetAllWorkshopData()[ModParameters.PackageId].GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            var card = enumerator.Current;
                            card.workshopName = battleCardDescRoot.cardDescList.Find(x => x.cardID == card.id.id)
                                .cardName;
                        }
                    }

                    typeof(ItemXmlDataList).GetField("_cardInfoTable", AccessTools.all)
                        .GetValue(ItemXmlDataList.instance);
                    using (var enumerator2 = ItemXmlDataList.instance.GetCardList()
                               .FindAll(x => x.id.packageId == ModParameters.PackageId).GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            var card = enumerator2.Current;
                            card.workshopName = battleCardDescRoot.cardDescList.Find(x => x.cardID == card.id.id)
                                .cardName;
                            ItemXmlDataList.instance.GetCardItem(card.id).workshopName = card.workshopName;
                        }
                    }
                }

            files = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language + "/BattleDialog")
                .GetFiles();
            var dialogDictionary =
                (Dictionary<string, BattleDialogRoot>)BattleDialogXmlList.Instance.GetType()
                    .GetField("_dictionary", AccessTools.all)
                    ?.GetValue(BattleDialogXmlList.Instance);
            foreach (var t in files)
                using (var stringReader = new StringReader(File.ReadAllText(t.FullName)))
                {
                    var battleDialogList =
                        ((BattleDialogRoot)new XmlSerializer(typeof(BattleDialogRoot))
                            .Deserialize(stringReader)).characterList;
                    foreach (var dialog in battleDialogList)
                    {
                        dialog.workshopId = ModParameters.PackageId;
                        dialog.bookId = int.Parse(dialog.characterID);
                    }

                    if (!dialogDictionary.ContainsKey("Workshop")) continue;
                    dialogDictionary["Workshop"].characterList
                        .RemoveAll(x => x.workshopId.Equals(ModParameters.PackageId));
                    if (dialogDictionary.ContainsKey("Workshop"))
                    {
                        dialogDictionary["Workshop"].characterList.AddRange(battleDialogList);
                    }
                    else
                    {
                        var battleDialogRoot = new BattleDialogRoot
                        {
                            groupName = "Workshop",
                            characterList = battleDialogList
                        };
                        dialogDictionary.Add("Workshop", battleDialogRoot);
                    }
                }

            files = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language + "/CharactersName")
                .GetFiles();
            ModParameters.NameTexts.Clear();
            foreach (var t in files)
                using (var stringReader3 = new StringReader(File.ReadAllText(t.FullName)))
                {
                    var charactersNameRoot =
                        (CharactersNameRoot)new XmlSerializer(typeof(CharactersNameRoot)).Deserialize(
                            stringReader3);
                    using (var enumerator3 =
                           Singleton<EnemyUnitClassInfoList>.Instance.GetAllWorkshopData()[ModParameters.PackageId]
                               .GetEnumerator())
                    {
                        while (enumerator3.MoveNext())
                        {
                            var enemy = enumerator3.Current;
                            enemy.name = charactersNameRoot.nameList.Find(x => x.ID == enemy.id.id).name;
                            Singleton<EnemyUnitClassInfoList>.Instance.GetData(enemy.id).name = enemy.name;
                            ModParameters.NameTexts.Add(enemy.id.id.ToString(), enemy.name);
                        }
                    }
                }

            files = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language + "/Books").GetFiles();
            foreach (var t in files)
                using (var stringReader4 = new StringReader(File.ReadAllText(t.FullName)))
                {
                    var bookDescRoot =
                        (BookDescRoot)new XmlSerializer(typeof(BookDescRoot)).Deserialize(stringReader4);
                    using (var enumerator4 =
                           Singleton<BookXmlList>.Instance.GetAllWorkshopData()[ModParameters.PackageId]
                               .GetEnumerator())
                    {
                        while (enumerator4.MoveNext())
                        {
                            var bookXml = enumerator4.Current;
                            bookXml.InnerName = bookDescRoot.bookDescList.Find(x => x.bookID == bookXml.id.id)
                                .bookName;
                        }
                    }

                    using (var enumerator5 = Singleton<BookXmlList>.Instance.GetList()
                               .FindAll(x => x.id.packageId == ModParameters.PackageId).GetEnumerator())
                    {
                        while (enumerator5.MoveNext())
                        {
                            var bookXml = enumerator5.Current;
                            bookXml.InnerName = bookDescRoot.bookDescList.Find(x => x.bookID == bookXml.id.id)
                                .bookName;
                            Singleton<BookXmlList>.Instance.GetData(bookXml.id).InnerName = bookXml.InnerName;
                        }
                    }

                    (typeof(BookDescXmlList).GetField("_dictionaryWorkshop", AccessTools.all)
                            .GetValue(Singleton<BookDescXmlList>.Instance) as Dictionary<string, List<BookDesc>>)
                        [ModParameters.PackageId] = bookDescRoot.bookDescList;
                }

            files = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language + "/DropBooks")
                .GetFiles();
            foreach (var t in files)
                using (var stringReader5 = new StringReader(File.ReadAllText(t.FullName)))
                {
                    var charactersNameRoot2 =
                        (CharactersNameRoot)new XmlSerializer(typeof(CharactersNameRoot)).Deserialize(
                            stringReader5);
                    using (var enumerator6 =
                           Singleton<DropBookXmlList>.Instance.GetAllWorkshopData()[ModParameters.PackageId]
                               .GetEnumerator())
                    {
                        while (enumerator6.MoveNext())
                        {
                            var dropBook = enumerator6.Current;
                            dropBook.workshopName =
                                charactersNameRoot2.nameList.Find(x => x.ID == dropBook.id.id).name;
                        }
                    }

                    using (var enumerator7 = Singleton<DropBookXmlList>.Instance.GetList()
                               .FindAll(x => x.id.packageId == ModParameters.PackageId).GetEnumerator())
                    {
                        while (enumerator7.MoveNext())
                        {
                            var dropBook = enumerator7.Current;
                            dropBook.workshopName =
                                charactersNameRoot2.nameList.Find(x => x.ID == dropBook.id.id).name;
                            Singleton<DropBookXmlList>.Instance.GetData(dropBook.id).workshopName =
                                dropBook.workshopName;
                        }
                    }
                }

            files = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language + "/StageName")
                .GetFiles();
            foreach (var t in files)
                using (var stringReader6 = new StringReader(File.ReadAllText(t.FullName)))
                {
                    var charactersNameRoot3 =
                        (CharactersNameRoot)new XmlSerializer(typeof(CharactersNameRoot)).Deserialize(
                            stringReader6);
                    using (var enumerator8 =
                           Singleton<StageClassInfoList>.Instance.GetAllWorkshopData()[ModParameters.PackageId]
                               .GetEnumerator())
                    {
                        while (enumerator8.MoveNext())
                        {
                            var stage = enumerator8.Current;
                            stage.stageName = charactersNameRoot3.nameList.Find(x => x.ID == stage.id.id).name;
                        }
                    }
                }

            files = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language + "/PassiveDesc")
                .GetFiles();
            foreach (var t in files)
                using (var stringReader7 = new StringReader(File.ReadAllText(t.FullName)))
                {
                    var passiveDescRoot =
                        (PassiveDescRoot)new XmlSerializer(typeof(PassiveDescRoot)).Deserialize(stringReader7);
                    using (var enumerator9 = Singleton<PassiveXmlList>.Instance.GetDataAll()
                               .FindAll(x => x.id.packageId == ModParameters.PackageId).GetEnumerator())
                    {
                        while (enumerator9.MoveNext())
                        {
                            var passive = enumerator9.Current;
                            passive.name = passiveDescRoot.descList.Find(x => x.ID == passive.id.id).name;
                            passive.desc = passiveDescRoot.descList.Find(x => x.ID == passive.id.id).desc;
                        }
                    }
                }

            var cardAbilityDictionary = typeof(BattleCardAbilityDescXmlList).GetField("_dictionary", AccessTools.all)
                    ?.GetValue(Singleton<BattleCardAbilityDescXmlList>.Instance) as
                Dictionary<string, BattleCardAbilityDesc>;
            files = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language +
                                      "/BattleCardAbilities").GetFiles();
            foreach (var t in files)
                using (var stringReader8 = new StringReader(File.ReadAllText(t.FullName)))
                {
                    foreach (var battleCardAbilityDesc in
                             ((BattleCardAbilityDescRoot)new XmlSerializer(typeof(BattleCardAbilityDescRoot))
                                 .Deserialize(stringReader8)).cardDescList)
                    {
                        cardAbilityDictionary.Remove(battleCardAbilityDesc.id);
                        cardAbilityDictionary.Add(battleCardAbilityDesc.id, battleCardAbilityDesc);
                    }
                }
        }

        public static void RemoveError()
        {
            var list = new List<string>();
            var list2 = new List<string>();
            list.Add("0Harmony");
            list.Add("NAudio");
            list.Add("Mono.Cecil");
            list.Add("MonoMod.Utils");
            list.Add("MonoMod.RuntimeDetour");
            using (var enumerator = Singleton<ModContentManager>.Instance.GetErrorLogs().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var errorLog = enumerator.Current;
                    if (list.Exists(x => errorLog.Contains(x))) list2.Add(errorLog);
                }
            }

            foreach (var item in list2) Singleton<ModContentManager>.Instance.GetErrorLogs().Remove(item);
        }
    }
}