using System.Collections.Generic;
using System.Linq;
using CustomMapUtility;
using LOR_XML;
using OmoriMod_Om21341.BLL_Om21341;
using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Passives;
using OmoriMod_Om21341.Omori_Om21341.MapManagers;
using UnityEngine;
using UtilLoader21341.Extensions;
using UtilLoader21341.Models;
using UtilLoader21341.Util;

namespace OmoriMod_Om21341.Omori_Om21341
{
    public static class OmoriUtil
    {
        public static void PrepareUnitsPassives(BattleUnitModel unit)
        {
            unit.passiveDetail.DestroyPassive(
                unit.passiveDetail.PassiveList.FirstOrDefault(x => x is PassiveAbility_EmotionalBurst_Om21341));
            switch (unit.UnitData.unitData.OwnerSephirah)
            {
                case SephirahType.Malkuth:
                    unit.passiveDetail.AddPassive(new LorId(OmoriModParameters.PackageId, 46));
                    break;
                case SephirahType.Yesod:
                    unit.passiveDetail.AddPassive(new LorId(OmoriModParameters.PackageId, 28));
                    break;
                case SephirahType.Hod:
                    unit.passiveDetail.AddPassive(new LorId(OmoriModParameters.PackageId, 47));
                    break;
                case SephirahType.Netzach:
                    unit.passiveDetail.AddPassive(new LorId(OmoriModParameters.PackageId, 48));
                    break;
            }
        }

        public static void ChangeToOmoriEgoMap(bool oneTurnEgo = false)
        {
            MapUtil.ChangeMapGeneric<Omori5_Om21341MapManager>(CustomMapHandler.GetCMU(OmoriModParameters.PackageId),
                new MapModelRoot
                {
                    Stage = "Omori5_Om21341",
                    OriginalMapStageIds = new List<LorIdRoot>
                        { new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 8 } },
                    IsPlayer = true,
                    OneTurnEgo = oneTurnEgo,
                    Component = nameof(Omori5_Om21341MapManager),
                    Bgy = 0.55f
                });
        }

        public static void ChangeToOmoriEgoAttackMap()
        {
            MapUtil.ChangeMapGeneric<Omori2_Om21341MapManager>(CustomMapHandler.GetCMU(OmoriModParameters.PackageId),
                new MapModelRoot
                {
                    Stage = "Omori2_Om21341",
                    OriginalMapStageIds = new List<LorIdRoot>
                        { new LorIdRoot { PackageId = OmoriModParameters.PackageId, Id = 8 } },
                    IsPlayer = true,
                    OneTurnEgo = true,
                    Component = nameof(Omori2_Om21341MapManager),
                    Bgy = 0.55f
                });
        }

        public static void ChangeMapToSuccumbState(ref bool _succumbStatus, ref bool _mapChanged)
        {
            if (_succumbStatus) _succumbStatus = false;
            if (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.isEgo) return;
            _mapChanged = true;
            ChangeToOmoriEgoMap();
        }

        public static void ChangeMapToSuccumbState(ref bool _mapChanged)
        {
            if (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.isEgo) return;
            _mapChanged = true;
            ChangeToOmoriEgoMap();
        }

        public static void ChangeToEgoMap(LorId cardId, ref bool _attackMapUsed)
        {
            if (cardId != new LorId(OmoriModParameters.PackageId, 907) ||
                SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.isEgo) return;
            _attackMapUsed = true;
            ChangeToOmoriEgoAttackMap();
        }

        public static void ReturnFromEgoAttackMap(ref bool _attackMapUsed)
        {
            if (!_attackMapUsed) return;
            _attackMapUsed = false;
            MapUtil.ReturnFromEgoMap(CustomMapHandler.GetCMU(OmoriModParameters.PackageId), "Omori2_Om21341",
                new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) });
        }

        public static void ReturnFromEgoMap(ref bool _mapChanged)
        {
            if (!_mapChanged) return;
            _mapChanged = false;
            MapUtil.ReturnFromEgoMap(CustomMapHandler.GetCMU(OmoriModParameters.PackageId), "Omori5_Om21341",
                new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) },
                true);
        }

        public static bool EgoActive(BattleUnitModel owner, ref bool _egoActive, List<AbnormalityCardDialog> dialog)
        {
            if (_egoActive) return false;
            _egoActive = true;
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_Immortal_DLL21341());
            owner.cardSlotDetail.RecoverPlayPoint(owner.cardSlotDetail.GetMaxPlayPoint());
            if (dialog.Any())
                UnitUtil.BattleAbDialog(owner.view.dialogUI, dialog, new Color(0.5f, 0, 0, 1f));
            return true;
        }
    }
}