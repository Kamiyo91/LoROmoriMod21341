using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BigDLL4221.Enum;
using BigDLL4221.Models;
using BigDLL4221.Utils;
using LOR_DiceSystem;
using OmoriMod_Om21341.BLL_Om21341;
using UnityEngine;

namespace OmoriMod_Om21341.OmoriHarmony_Om21341.Harmony
{
    public class OmoriInit_Om21341 : ModInitializer
    {
        public override void OnInitializeMod()
        {
            OnInitParameters();
            ArtUtil.GetArtWorks(new DirectoryInfo(OmoriModParameters.Path + "/ArtWork"));
            CardUtil.ChangeCardItem(ItemXmlDataList.instance, OmoriModParameters.PackageId);
            PassiveUtil.ChangePassiveItem(OmoriModParameters.PackageId);
            LocalizeUtil.AddGlobalLocalize(OmoriModParameters.PackageId);
            ArtUtil.PreLoadBufIcons();
            LocalizeUtil.RemoveError();
            CardUtil.InitKeywordsList(new List<Assembly> { Assembly.GetExecutingAssembly() });
            ArtUtil.InitCustomEffects(new List<Assembly> { Assembly.GetExecutingAssembly() });
            CustomMapHandler.ModResources.CacheInit.InitCustomMapFiles(Assembly.GetExecutingAssembly());
        }

        private static void OnInitParameters()
        {
            ModParameters.PackageIds.Add(OmoriModParameters.PackageId);
            OmoriModParameters.Path = Path.GetDirectoryName(
                Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            ModParameters.Path.Add(OmoriModParameters.PackageId, OmoriModParameters.Path);
            ModParameters.DefaultKeyword.Add(OmoriModParameters.PackageId, "OmoriModPage_Om21341");
            OnInitSprites();
            OnInitKeypages();
            OnInitCards();
            OnInitDropBooks();
            OnInitPassives();
            OnInitRewards();
            OnInitStages();
            OnInitCredenza();
        }

        private static void OnInitRewards()
        {
            ModParameters.StartUpRewardOptions.Add(new RewardOptions(new Dictionary<LorId, int>
                {
                    { new LorId(OmoriModParameters.PackageId, 10), 0 }
                }
            ));
        }

        private static void OnInitCards()
        {
            ModParameters.CardOptions.Add(OmoriModParameters.PackageId, new List<CardOptions>
            {
                new CardOptions(32, CardOption.Personal,
                    cardColorOptions: new CardColorOptions(new Color(1f, 0.6f, 0.6f),
                        customIconColor: new Color(1f, 0.6f, 0.6f), useHSVFilter: false)),
                new CardOptions(33, CardOption.Personal,
                    cardColorOptions: new CardColorOptions(new Color(0.6f, 0.8f, 1f),
                        customIconColor: new Color(0.6f, 0.8f, 0.6f), useHSVFilter: false)),
                new CardOptions(34, CardOption.Personal,
                    cardColorOptions: new CardColorOptions(new Color(1f, 1f, 0.6f),
                        customIconColor: new Color(1f, 1f, 0.6f), useHSVFilter: false)),
                new CardOptions(35, CardOption.Personal,
                    cardColorOptions: new CardColorOptions(Color.gray, customIconColor: Color.gray,
                        useHSVFilter: false)),
                new CardOptions(66, CardOption.EgoPersonal),
                new CardOptions(67, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) }),
                new CardOptions(68, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) }),
                new CardOptions(69, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) }),
                new CardOptions(70, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) }),
                new CardOptions(71, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) }),
                new CardOptions(72, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) }),
                new CardOptions(73, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) }),
                new CardOptions(74, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) },
                    cardColorOptions: new CardColorOptions(Color.gray, customIconColor: Color.gray,
                        useHSVFilter: false)),
                new CardOptions(75, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) },
                    cardColorOptions: new CardColorOptions(Color.gray, customIconColor: Color.gray,
                        useHSVFilter: false)),
                new CardOptions(76, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) }),
                new CardOptions(77, CardOption.OnlyPage, new List<string> { "OmoriPage_Om21341" },
                    new List<LorId> { new LorId(OmoriModParameters.PackageId, 10000001) },
                    cardColorOptions: new CardColorOptions(Color.gray, customIconColor: Color.gray,
                        useHSVFilter: false))
            });
        }

        private static void OnInitKeypages()
        {
            ModParameters.KeypageOptions.Add(OmoriModParameters.PackageId, new List<KeypageOptions>
            {
                new KeypageOptions(10000001, bookCustomOptions: new BookCustomOptions(nameTextId: 12),
                    keypageColorOptions: new KeypageColorOptions(Color.gray, Color.gray)),
                new KeypageOptions(12, bookCustomOptions: new BookCustomOptions(nameTextId: 12),
                    keypageColorOptions: new KeypageColorOptions(Color.gray, Color.gray)),
                new KeypageOptions(10000015,
                    keypageColorOptions: new KeypageColorOptions(new Color(1f, 0.6f, 0.6f), new Color(1f, 0.6f, 0.6f))),
                new KeypageOptions(10000016,
                    keypageColorOptions: new KeypageColorOptions(new Color(0.6f, 0.8f, 1f), new Color(0.6f, 0.8f, 1f))),
                new KeypageOptions(10000017,
                    keypageColorOptions: new KeypageColorOptions(new Color(1f, 1f, 0.6f), new Color(1f, 1f, 0.6f))),
                new KeypageOptions(10000018, keypageColorOptions: new KeypageColorOptions(Color.gray, Color.gray))
            });
        }

        private static void OnInitCredenza()
        {
            ModParameters.CredenzaOptions.Add(OmoriModParameters.PackageId,
                new CredenzaOptions(CredenzaEnum.ModifiedCredenza, credenzaNameId: OmoriModParameters.PackageId,
                    customIconSpriteId: OmoriModParameters.PackageId, credenzaBooksId: new List<int>
                    {
                        10000001
                    }));
        }

        private static void OnInitSprites()
        {
            ModParameters.SpriteOptions.Add(OmoriModParameters.PackageId, new List<SpriteOptions>
            {
                new SpriteOptions(SpriteEnum.Custom, 10000001, "OmoriDefault_Om21341"),
                new SpriteOptions(SpriteEnum.Custom, 10000015, "FragmentDefault_Om21341"),
                new SpriteOptions(SpriteEnum.Custom, 10000016, "FragmentDefault_Om21341"),
                new SpriteOptions(SpriteEnum.Custom, 10000017, "FragmentDefault_Om21341"),
                new SpriteOptions(SpriteEnum.Custom, 10000018, "FragmentDefault_Om21341")
            });
        }

        private static void OnInitStages()
        {
            ModParameters.StageOptions.Add(OmoriModParameters.PackageId, new List<StageOptions>
            {
                new StageOptions(8, preBattleOptions: new PreBattleOptions(
                        new List<SephirahType>
                            { SephirahType.Hod, SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Netzach },
                        battleType: PreBattleType.SephirahUnits,
                        sephirahUnits: new List<SephirahType>
                            { SephirahType.Hod, SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Netzach }),
                    stageRewardOptions: new RewardOptions(
                        new Dictionary<LorId, int> { { new LorId(OmoriModParameters.PackageId, 8), 5 } },
                        messageId: "OmoriDropBook_Om21341"),
                    stageColorOptions: new StageColorOptions(Color.gray, Color.gray))
            });
        }

        private static void OnInitPassives()
        {
            ModParameters.PassiveOptions.Add(OmoriModParameters.PackageId, new List<PassiveOptions>
            {
                new PassiveOptions(28,
                    cannotBeUsedWithPassives: new List<LorId>
                    {
                        new LorId(OmoriModParameters.PackageId, 46), new LorId(OmoriModParameters.PackageId, 47),
                        new LorId(OmoriModParameters.PackageId, 48)
                    }),
                new PassiveOptions(46,
                    cannotBeUsedWithPassives: new List<LorId>
                        { new LorId(OmoriModParameters.PackageId, 47), new LorId(OmoriModParameters.PackageId, 48) }),
                new PassiveOptions(47,
                    cannotBeUsedWithPassives: new List<LorId> { new LorId(OmoriModParameters.PackageId, 48) }),
                new PassiveOptions(35, false),
                new PassiveOptions(51, false,
                    cannotBeUsedWithPassives: new List<LorId>
                    {
                        new LorId(OmoriModParameters.PackageId, 28), new LorId(OmoriModParameters.PackageId, 46),
                        new LorId(OmoriModParameters.PackageId, 47), new LorId(OmoriModParameters.PackageId, 48)
                    },
                    passiveColorOptions: new PassiveColorOptions(Color.gray, Color.gray)),
                new PassiveOptions(53, false)
            });
        }

        private static void OnInitDropBooks()
        {
            ModParameters.DropBookOptions.Add(OmoriModParameters.PackageId, new List<DropBookOptions>
            {
                new DropBookOptions(11, new DropBookColorOptions(Color.gray, Color.gray))
            });
        }
    }
}