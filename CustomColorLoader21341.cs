using System.Collections.Generic;
using CustomColorUtil.Models;
using OmoriMod_Om21341.BLL_Om21341;

namespace OmoriMod_Om21341
{
    public static class CustomColorLoader21341
    {
        public static List<KeypageOptionRoot> KeypageOptionRoot()
        {
            return new List<KeypageOptionRoot>
            {
                new KeypageOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 10000001, 12, 10000018 },
                    KeypageColorOptions = new ColorOptionsRoot
                    {
                        FrameColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 },
                        TextColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 }
                    }
                },
                new KeypageOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 10000015 },
                    KeypageColorOptions = new ColorOptionsRoot
                    {
                        FrameColor = new ColorRoot { R = 255, G = 153, B = 153, A = 255 },
                        TextColor = new ColorRoot { R = 255, G = 153, B = 153, A = 255 }
                    }
                },
                new KeypageOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 10000017 },
                    KeypageColorOptions = new ColorOptionsRoot
                    {
                        FrameColor = new ColorRoot { R = 153, G = 204, B = 255, A = 255 },
                        TextColor = new ColorRoot { R = 153, G = 204, B = 255, A = 255 }
                    }
                },
                new KeypageOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 10000016 },
                    KeypageColorOptions = new ColorOptionsRoot
                    {
                        FrameColor = new ColorRoot { R = 255, G = 255, B = 153, A = 255 },
                        TextColor = new ColorRoot { R = 255, G = 255, B = 153, A = 255 }
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
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 28 },
                    PassiveColorOptions = new ColorOptionsRoot
                    {
                        FrameColor = new ColorRoot { R = 255, G = 153, B = 153, A = 255 },
                        TextColor = new ColorRoot { R = 255, G = 153, B = 153, A = 255 }
                    }
                },
                new PassiveOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 46 },
                    PassiveColorOptions = new ColorOptionsRoot
                    {
                        FrameColor = new ColorRoot { R = 255, G = 255, B = 153, A = 255 },
                        TextColor = new ColorRoot { R = 255, G = 255, B = 153, A = 255 }
                    }
                },
                new PassiveOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 47 },
                    PassiveColorOptions = new ColorOptionsRoot
                    {
                        FrameColor = new ColorRoot { R = 153, G = 204, B = 255, A = 255 },
                        TextColor = new ColorRoot { R = 153, G = 204, B = 255, A = 255 }
                    }
                },
                new PassiveOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 48, 51 },
                    PassiveColorOptions = new ColorOptionsRoot
                    {
                        FrameColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 },
                        TextColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 }
                    }
                }
            };
        }

        public static List<DropBookOptionRoot> DropBookOptionRoot()
        {
            return new List<DropBookOptionRoot>
            {
                new DropBookOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 11 }, DropBookColorOptions =
                        new ColorOptionsRoot
                        {
                            FrameColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 },
                            TextColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 }
                        }
                }
            };
        }

        public static List<StageOptionRoot> StageOptionRoot()
        {
            return new List<StageOptionRoot>
            {
                new StageOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 8 }, StageColorOptions =
                        new ColorOptionsRoot
                        {
                            FrameColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 },
                            TextColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 }
                        }
                }
            };
        }

        public static List<CategoryOptionRoot> CategoryOptionRoot()
        {
            return new List<CategoryOptionRoot>
            {
                new CategoryOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, AdditionalValue = "_1",
                    BookDataColor = new ColorOptionsRoot
                    {
                        FrameColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 },
                        TextColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 }
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
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 35, 74, 75, 77 },
                    CardColorOptions = new CardColorOptionRoot
                    {
                        CardColor = new ColorRoot { R = 127, G = 127, B = 127, A = 255 },
                        IconColor = new HsvColorRoot { H = 0, S = 0, V = 0.05f }
                    }
                },
                new CardOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 32 },
                    CardColorOptions = new CardColorOptionRoot
                    {
                        CardColor = new ColorRoot { R = 255, G = 153, B = 153, A = 255 },
                        CustomIconColor = new ColorRoot { R = 255, G = 153, B = 153, A = 255 }, UseHSVFilter = false
                    }
                },
                new CardOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 33 },
                    CardColorOptions = new CardColorOptionRoot
                    {
                        CardColor = new ColorRoot { R = 153, G = 204, B = 255, A = 255 },
                        CustomIconColor = new ColorRoot { R = 153, G = 204, B = 255, A = 255 }, UseHSVFilter = false
                    }
                },
                new CardOptionRoot
                {
                    PackageId = OmoriModParameters.PackageId, Ids = new List<int> { 34 },
                    CardColorOptions = new CardColorOptionRoot
                    {
                        CardColor = new ColorRoot { R = 255, G = 255, B = 153, A = 255 },
                        CustomIconColor = new ColorRoot { R = 255, G = 255, B = 153, A = 255 }, UseHSVFilter = false
                    }
                }
            };
        }
    }
}