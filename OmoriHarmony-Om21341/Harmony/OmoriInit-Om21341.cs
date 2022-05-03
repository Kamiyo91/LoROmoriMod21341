using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BLL_Om21341;
using KamiyoStaticBLL.Enums;
using KamiyoStaticBLL.Models;
using KamiyoStaticUtil.Utils;
using MonoMod.Utils;
using Omori_Om21341;

namespace OmoriHarmony_Om21341.Harmony
{
    public class OmoriInit_Om21341 : ModInitializer
    {
        private const string PackageId = "OmoriModOm21341.Mod";

        public override void OnInitializeMod()
        {
            InitParameters();
            UnitUtil.ChangeCardItem(ItemXmlDataList.instance, OmoriModParameters.PackageId);
            UnitUtil.ChangePassiveItem(OmoriModParameters.PackageId);
            SkinUtil.PreLoadBufIcons();
            LocalizeUtil.AddLocalLocalize(OmoriModParameters.Path, OmoriModParameters.PackageId);
            LocalizeUtil.RemoveError();
            UnitUtil.InitKeywords(OmoriOm21341GetAssembly.GetAssembly());
        }

        private static void InitParameters()
        {
            OmoriModParameters.PackageId = PackageId;
            OmoriModParameters.Path =
                Path.GetDirectoryName(
                    Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            ModParameters.PackageIds.Add(OmoriModParameters.PackageId);
            ModParameters.Path.Add(OmoriModParameters.Path);
            MapStaticUtil.GetArtWorks(new DirectoryInfo(OmoriModParameters.Path + "/ArtWork"));
            ModParameters.DefaultKeyword.Add(OmoriModParameters.PackageId, "OmoriModPage_Om21341");
            ModParameters.OnlyCardKeywords.AddRange(new List<Tuple<List<string>, List<LorId>, LorId>>
            {
                new Tuple<List<string>, List<LorId>, LorId>(new List<string> { "OmoriPage_Om21341" },
                    new List<LorId>
                    {
                        new LorId(PackageId, 67), new LorId(PackageId, 68), new LorId(PackageId, 69),
                        new LorId(PackageId, 70), new LorId(PackageId, 72), new LorId(PackageId, 74),
                        new LorId(PackageId, 75), new LorId(PackageId, 76), new LorId(PackageId, 77),
                        new LorId(PackageId, 73), new LorId(PackageId, 71)
                    }, new LorId(PackageId, 10000001))
            });
            ModParameters.DynamicNames.AddRange(new Dictionary<LorId, LorId>
            {
                { new LorId(PackageId, 10000001), new LorId(PackageId, 12) }
            });
            ModParameters.PersonalCardList.AddRange(new List<LorId>
            {
                new LorId(PackageId, 32), new LorId(PackageId, 33), new LorId(PackageId, 34), new LorId(PackageId, 35)
            });
            ModParameters.EgoPersonalCardList.AddRange(new List<LorId>
            {
                new LorId(PackageId, 66)
            });
            ModParameters.UntransferablePassives.AddRange(new List<LorId>
            {
                new LorId(PackageId, 35), new LorId(PackageId, 51), new LorId(PackageId, 53)
            });
            ModParameters.SpritePreviewChange.AddRange(new Dictionary<string, List<LorId>>
            {
                {
                    "FragmentDefault_Om21341",
                    new List<LorId>
                    {
                        new LorId(PackageId, 10000015), new LorId(PackageId, 10000016), new LorId(PackageId, 10000017),
                        new LorId(PackageId, 10000018)
                    }
                },
                { "OmoriDefault_Om21341", new List<LorId> { new LorId(PackageId, 10000001) } }
            });
            ModParameters.SameInnerIdPassives.AddRange(new Dictionary<int, List<LorId>>
            {
                {
                    21341,
                    new List<LorId>
                    {
                        new LorId(PackageId, 28), new LorId(PackageId, 46), new LorId(PackageId, 47),
                        new LorId(PackageId, 48)
                    }
                }
            });
            ModParameters.ExtraReward.AddRange(new Dictionary<LorId, ExtraRewards>
            {
                {
                    new LorId(PackageId, 8),
                    new ExtraRewards
                    {
                        MessageId = "OmoriDropBook_Om21341",
                        DroppedBooks = new List<DropIdQuantity>
                            { new DropIdQuantity { BookId = new LorId(PackageId, 11), Quantity = 5 } }
                    }
                }
            });
            ModParameters.UniqueUnitStages.AddRange(new Dictionary<LorId, List<SephirahType>>
            {
                {
                    new LorId(PackageId, 8), new List<SephirahType>
                        { SephirahType.Hod, SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Netzach }
                }
            });
            ModParameters.BooksIds.AddRange(new List<LorId>
            {
                new LorId(PackageId, 10000001)
            });
            ModParameters.ExtraConditionPassives.AddRange(new List<Tuple<LorId, LorId>>
            {
                new Tuple<LorId, LorId>(new LorId(PackageId, 28), new LorId(PackageId, 51)),
                new Tuple<LorId, LorId>(new LorId(PackageId, 46), new LorId(PackageId, 51)),
                new Tuple<LorId, LorId>(new LorId(PackageId, 47), new LorId(PackageId, 51)),
                new Tuple<LorId, LorId>(new LorId(PackageId, 48), new LorId(PackageId, 51))
            });
            ModParameters.PreBattleUnits.AddRange(
                new List<Tuple<LorId, List<PreBattleUnitModel>, List<SephirahType>, PreBattleUnitSpecialCases>>
                {
                    new Tuple<LorId, List<PreBattleUnitModel>, List<SephirahType>, PreBattleUnitSpecialCases>(
                        new LorId(PackageId, 8), null, new List<SephirahType>
                        {
                            SephirahType.Hod,
                            SephirahType.Yesod,
                            SephirahType.Netzach,
                            SephirahType.Malkuth
                        }, PreBattleUnitSpecialCases.Sephirah4)
                });
            ModParameters.BookList.AddRange(new List<LorId>
            {
                new LorId(PackageId, 10)
            });
        }
    }
}