using System.Collections.Generic;
using OmoriMod_Om21341.Omori_Om21341.MapManagers;
using UtilLoader21341.Models;

namespace OmoriMod_Om21341.BLL_Om21341
{
    public static class OmoriModParameters
    {
        public static string PackageId = "OmoriModOm21341.Mod";
        public static string Path = string.Empty;
        public static BlackSilence4thMapManager BoomEffectMap = null;

        public static MapModelRoot OmoriMap1 = new MapModelRoot
        {
            Stage = "Omori1_Om21341",
            OriginalMapStageIds = new List<LorIdRoot>
                { new LorIdRoot { PackageId = PackageId, Id = 8 } },
            Component = nameof(Omori1_Om21341MapManager),
            Bgy = 0.55f
        };

        public static MapModelRoot OmoriMap2 = new MapModelRoot
        {
            Stage = "Omori2_Om21341",
            OriginalMapStageIds = new List<LorIdRoot>
                { new LorIdRoot { PackageId = PackageId, Id = 8 } },
            Component = nameof(Omori2_Om21341MapManager),
            Bgy = 0.55f
        };

        public static MapModelRoot OmoriMap3 = new MapModelRoot
        {
            Stage = "Omori3_Om21341",
            OriginalMapStageIds = new List<LorIdRoot>
                { new LorIdRoot { PackageId = PackageId, Id = 8 } },
            Component = nameof(Omori3_Om21341MapManager),
            Bgy = 0.55f
        };

        public static MapModelRoot OmoriMap4 = new MapModelRoot
        {
            Stage = "Omori4_Om21341",
            OriginalMapStageIds = new List<LorIdRoot>
                { new LorIdRoot { PackageId = PackageId, Id = 8 } },
            Component = nameof(Omori4_Om21341MapManager),
            Bgy = 0.55f
        };

        public static MapModelRoot OmoriMap5 = new MapModelRoot
        {
            Stage = "Omori5_Om21341",
            OriginalMapStageIds = new List<LorIdRoot>
                { new LorIdRoot { PackageId = PackageId, Id = 8 } },
            Component = nameof(Omori5_Om21341MapManager),
            Bgy = 0.55f
        };
    }
}