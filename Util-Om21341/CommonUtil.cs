using LOR_DiceSystem;
using OmoriMod_Om21341.BLL_Om21341;
using UnityEngine;

namespace OmoriMod_Om21341.Util_Om21341
{
    public static class CommonUtil
    {
        public static bool CantUseCardAfraid(BattleDiceCardModel card, string packageId)
        {
            return card.XmlData.Spec.Ranged == CardRange.FarArea ||
                   card.XmlData.Spec.Ranged == CardRange.FarAreaEach || card.GetOriginCost() > 3 ||
                   card.XmlData.IsEgo() || (card.XmlData.id.packageId == packageId &&
                                            (card.XmlData.id.id == 32 || card.XmlData.id.id == 33 ||
                                             card.XmlData.id.id == 34 ||
                                             card.XmlData.id.id == 35));
        }

        public static void LoadBoomEffect()
        {
            var map = Util.LoadPrefab("InvitationMaps/InvitationMap_BlackSilence4",
                SingletonBehavior<BattleSceneRoot>.Instance.transform);
            OmoriModParameters.BoomEffectMap = map.GetComponent<MapManager>() as BlackSilence4thMapManager;
            Object.Destroy(map);
        }

        public static void UnloadBoomEffect()
        {
            Object.Destroy(OmoriModParameters.BoomEffectMap);
            OmoriModParameters.BoomEffectMap = null;
        }
    }
}