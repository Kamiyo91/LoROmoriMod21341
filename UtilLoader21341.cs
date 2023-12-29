using System.Collections.Generic;
using LOR_DiceSystem;
using OmoriMod_Om21341.BLL_Om21341;
using UtilLoader21341.Enum;
using UtilLoader21341.Models;

namespace OmoriMod_Om21341
{
    public static class UtilLoader21341
    {
        public static List<SpriteOptionRoot> SpriteOptionRoot()
        {
            return new List<SpriteOptionRoot>
            {
                new SpriteOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 10000001 },
                    SpritePK = "OmoriDefault_Om21341"
                },
                new SpriteOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId,
                    Ids = new List<int> { 10000015, 10000016, 10000017, 10000018 },
                    SpritePK = "FragmentDefault_Om21341"
                }
            };
        }

        public static List<KeypageOptionRoot> KeypageOptionRoot()
        {
            return new List<KeypageOptionRoot>
            {
                new KeypageOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, KeypageId = 10000001, EveryoneCanEquip = true,
                    BookCustomOptions = new BookCustomOptionRoot
                    {
                        NameTextId = 12
                    }
                }
            };
        }

        public static List<SkinOptionRoot> SkinOptionRoot()
        {
            return new List<SkinOptionRoot>
            {
                new SkinOptionRoot { PackageId = OmoriModParameters.PackageId, SkinName = "Omori_Om21341" }
            };
        }

        public static List<CustomSkinOptionRoot> CustomSkinOptionRoot()
        {
            return new List<CustomSkinOptionRoot>
            {
                new CustomSkinOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, SkinName = "Omori_Om21341",
                    KeypageId = 10000001, CharacterNameId = 12
                }
            };
        }

        public static List<RewardOptionRoot> RewardOptionRoot()
        {
            return new List<RewardOptionRoot>
            {
                new RewardOptionRoot
                {
                    Books = new List<ItemQuantityRoot>
                    {
                        new ItemQuantityRoot
                        {
                            LorId = new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 10 }, Quantity = 99
                        }
                    }
                }
            };
        }

        public static List<CardOptionRoot> CardOptionRoot()
        {
            return new List<CardOptionRoot>
            {
                new CardOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId,
                    Ids = new List<int> { 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77 },
                    Option = CardOption.OnlyPage, Keywords = new List<string> { "OmoriPage_Om21341" },
                    BookId = new List<LorIdRoot>
                        { new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 10000001 } }
                },
                new CardOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 32, 33, 34, 35 },
                    Option = CardOption.Personal
                },
                new CardOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 66 },
                    Option = CardOption.EgoPersonal
                }
            };
        }

        public static List<StageOptionRoot> StageOptionRoot()
        {
            return new List<StageOptionRoot>
            {
                new StageOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId,
                    StageId = 8,
                    PreBattleOptions = new PreBattleOptionRoot
                    {
                        SephirahUnits = new List<SephiorahUnitsRoot>
                        {
                            new SephiorahUnitsRoot
                            {
                                Floor = SephirahType.Hod,
                                SephirahUnit = new List<SephirahType>
                                    { SephirahType.Hod, SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Netzach }
                            },
                            new SephiorahUnitsRoot
                            {
                                Floor = SephirahType.Malkuth,
                                SephirahUnit = new List<SephirahType>
                                    { SephirahType.Hod, SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Netzach }
                            },
                            new SephiorahUnitsRoot
                            {
                                Floor = SephirahType.Yesod,
                                SephirahUnit = new List<SephirahType>
                                    { SephirahType.Hod, SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Netzach }
                            },
                            new SephiorahUnitsRoot
                            {
                                Floor = SephirahType.Netzach,
                                SephirahUnit = new List<SephirahType>
                                    { SephirahType.Hod, SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Netzach }
                            }
                        },
                        BattleType = PreBattleType.SephirahUnits
                    },
                    StageRewardOptions = new RewardOptionRoot
                    {
                        Books = new List<ItemQuantityRoot>
                        {
                            new ItemQuantityRoot
                            {
                                LorId = new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 11 },
                                Quantity = 5
                            }
                        },
                        MessageId = "OmoriDropBook_Om21341"
                    }
                }
            };
        }

        public static List<PassiveOptionRoot> PassiveOptionRoot()
        {
            return new List<PassiveOptionRoot>
            {
                new PassiveOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, PassiveId = 28,
                    CannotBeUsedWithPassives = new List<LorIdRoot>
                    {
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 46 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 47 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 48 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 51 }
                    }
                },
                new PassiveOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, PassiveId = 46,
                    CannotBeUsedWithPassives = new List<LorIdRoot>
                    {
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 28 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 47 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 48 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 51 }
                    }
                },
                new PassiveOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, PassiveId = 47,
                    CannotBeUsedWithPassives = new List<LorIdRoot>
                    {
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 46 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 28 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 48 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 51 }
                    }
                },
                new PassiveOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, PassiveId = 48,
                    CannotBeUsedWithPassives = new List<LorIdRoot>
                    {
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 46 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 47 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 28 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 51 }
                    }
                },
                new PassiveOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, PassiveId = 51,
                    CannotBeUsedWithPassives = new List<LorIdRoot>
                    {
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 46 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 47 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 48 },
                        new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 28 }
                    }
                }
            };
        }

        public static DefaultKeywordOption DefaultKeywordOption()
        {
            return new DefaultKeywordOption
                { PackageId = OmoriModParameters.PackageId, Keyword = "OmoriModPage_Om21341" };
        }

        public static List<CategoryOptionRoot> CategoryOptionRoot()
        {
            return new List<CategoryOptionRoot>
            {
                new CategoryOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, AdditionalValue = "1",
                    CustomIconSpriteId = "OmoriModOm21341.Mod", CategoryNameId = "OmoriModOm21341.Mod",
                    CredenzaBooksId = new List<int>
                        { 10000001 },
                    CategoryBooksId = new List<int>
                    {
                        10000001, 10000015, 10000016, 10000017, 10000018
                    }
                }
            };
        }
    }
}