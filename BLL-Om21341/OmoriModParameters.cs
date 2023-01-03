using System.Collections.Generic;
using BigDLL4221.Models;
using OmoriMod_Om21341.Omori_Om21341.MapManagers;

namespace OmoriMod_Om21341.BLL_Om21341
{
    public static class OmoriModParameters
    {
        public static string PackageId = "OmoriModOm21341.Mod";
        public static string Path = string.Empty;
        public static BlackSilence4thMapManager BoomEffectMap = null;

        public static MapModel OmoriMap1 = new MapModel(typeof(Omori1_Om21341MapManager), "Omori1_Om21341",
            bgy: 0.55f,
            originalMapStageIds: new List<LorId> { new LorId(PackageId, 7), new LorId(PackageId, 8) });

        public static MapModel OmoriMap2 = new MapModel(typeof(Omori2_Om21341MapManager), "Omori2_Om21341",
            bgy: 0.55f,
            originalMapStageIds: new List<LorId> { new LorId(PackageId, 7), new LorId(PackageId, 8) });

        public static MapModel OmoriMap3 = new MapModel(typeof(Omori3_Om21341MapManager), "Omori3_Om21341",
            bgy: 0.55f,
            originalMapStageIds: new List<LorId> { new LorId(PackageId, 7), new LorId(PackageId, 8) });

        public static MapModel OmoriMap4 = new MapModel(typeof(Omori4_Om21341MapManager), "Omori4_Om21341",
            bgy: 0.55f,
            originalMapStageIds: new List<LorId> { new LorId(PackageId, 7), new LorId(PackageId, 8) });

        public static MapModel OmoriMap5 = new MapModel(typeof(Omori5_Om21341MapManager), "Omori5_Om21341",
            bgy: 0.55f,
            originalMapStageIds: new List<LorId> { new LorId(PackageId, 7), new LorId(PackageId, 8) });
    }
}