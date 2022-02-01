﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace BLL_Om21341.Models
{
    public static class ModParameters
    {
        public const string PackageId = "OmoriModOm21341.Mod";

        public const int PreBattleUnits = 1;
        public static string Path;
        public static readonly Dictionary<string, Sprite> ArtWorks = new Dictionary<string, Sprite>();
        public static string Language;
        public static Dictionary<string, EffectTextModel> EffectTexts = new Dictionary<string, EffectTextModel>();
        public static Dictionary<string, string> NameTexts = new Dictionary<string, string>();

        public static List<Tuple<List<string>, List<int>, int>> OnlyCardKeywords =
            new List<Tuple<List<string>, List<int>, int>>
            {
                new Tuple<List<string>, List<int>, int>(new List<string> { "OmoriPage_Om21341" },
                    new List<int> { 67, 68, 69, 70, 72, 74, 75, 76, 77 }, 10000001),
                new Tuple<List<string>, List<int>, int>(new List<string> { "OmoriPage_Om21341", "FriendsPage_Om21341" },
                    new List<int> { 73, 71 }, 10000001)
            };

        public static BlackSilence4thMapManager BoomEffectMap = null;

        public static readonly Dictionary<int, int> DynamicNames = new Dictionary<int, int>
        {
            { 10000001, 12 }
        };

        public static readonly List<int> PersonalCardList = new List<int>
            { 32, 33, 34, 35 };

        public static readonly List<int> EgoPersonalCardList = new List<int> { 66 };
        public static readonly List<int> UntransferablePassives = new List<int> { 35, 51, 53 };


        public static readonly Dictionary<string, List<int>> SpritePreviewChange = new Dictionary<string, List<int>>
        {
            { "FragmentDefault_Om21341", new List<int> { 10000015, 10000016, 10000017, 10000018 } },
            { "OmoriDefault_Om21341", new List<int> { 10000001 } }
        };

        public static readonly Dictionary<int, List<int>> SameInnerIdPassives = new Dictionary<int, List<int>>
        {
            { 21341, new List<int> { 28, 46, 47, 48 } }
        };

        public static readonly Dictionary<int, ExtraRewards> ExtraReward = new Dictionary<int, ExtraRewards>
        {
            {
                8,
                new ExtraRewards
                {
                    MessageId = "OmoriDropBook_Om21341",
                    DroppedBooks = new List<DropIdQuantity> { new DropIdQuantity { BookId = 11, Quantity = 5 } }
                }
            }
        };

        public static readonly Dictionary<int, List<SephirahType>> UniqueUnitStages =
            new Dictionary<int, List<SephirahType>>
            {
                {
                    8,
                    new List<SephirahType>
                        { SephirahType.Hod, SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Netzach }
                }
            };

        public static readonly List<int> BooksIds = new List<int>
        {
            10000001
        };

        public static readonly List<Tuple<LorId, LorId>> ExtraConditionPassives = new List<Tuple<LorId, LorId>>
        {
            new Tuple<LorId, LorId>(new LorId(PackageId, 28), new LorId(PackageId, 51)),
            new Tuple<LorId, LorId>(new LorId(PackageId, 46), new LorId(PackageId, 51)),
            new Tuple<LorId, LorId>(new LorId(PackageId, 47), new LorId(PackageId, 51)),
            new Tuple<LorId, LorId>(new LorId(PackageId, 48), new LorId(PackageId, 51))
        };
    }
}