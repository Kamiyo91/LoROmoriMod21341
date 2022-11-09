using System.Collections.Generic;
using System.Linq;
using BigDLL4221.BaseClass;
using BigDLL4221.Buffs;
using BigDLL4221.Models;
using BigDLL4221.Utils;
using OmoriMod_Om21341.BLL_Om21341;
using OmoriMod_Om21341.BLL_Om21341.Extensions.MechUtilModelExtensions;
using OmoriMod_Om21341.Omori_Om21341.MapManagers;

namespace OmoriMod_Om21341.Omori_Om21341.MechUtil
{
    public class MechUtil_Omori : MechUtilBase
    {
        private readonly MechUtil_OmoriModel _model;

        public MechUtil_Omori(MechUtil_OmoriModel model) : base(model)
        {
            _model = model;
        }

        public override void SurviveCheck(int dmg)
        {
            if (_model.Owner.hp - dmg > _model.SurviveHp || !_model.Survive) return;
            _model.Survive = false;
            _model.RechargeCount = 0;
            UnitUtil.UnitReviveAndRecovery(_model.Owner, 0, _model.RecoverLightOnSurvive);
            _model.Owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_Immortal_DLL4221());
            _model.Owner.SetHp(_model.RecoverToHp);
            _model.Owner.bufListDetail.AddBufWithoutDuplication(
                new BattleUnitBuf_ImmunityToStatusAlimentType_DLL4221());
            SetSuccumbStatus(true);
        }

        public bool GetSuccumbStatus()
        {
            return _model.NotSuccumb;
        }

        public void RechargeCheck()
        {
            if (_model.RechargeCount > 4 && !_model.Survive)
                _model.Survive = true;
        }

        public void IncreaseRecharge()
        {
            if (_model.RechargeCount < 5) _model.RechargeCount++;
        }

        public void SetSuccumbStatus(bool value)
        {
            _model.NotSuccumb = value;
        }

        public void ChangeMap(BattleUnitModel owner)
        {
            if (owner.faction != Faction.Player ||
                SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.isEgo) return;
            _model.MapChanged = true;
            ChangeToOmoriEgoMap();
        }

        private static void ChangeToOmoriEgoMap()
        {
            MapUtil.ChangeMap(new MapModel
            {
                Stage = "Omori5_Om21341",
                OriginalMapStageIds = new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) },
                IsPlayer = true,
                OneTurnEgo = false,
                Component = typeof(Omori5_Om21341MapManager),
                Bgy = 0.55f
            });
        }

        public override bool EgoActive()
        {
            if (!_model.EgoOptions.TryGetValue(0, out var egoOptions)) return false;
            _model.Owner.bufListDetail.AddBufWithoutDuplication(egoOptions.EgoType);
            _model.Owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_Immortal_DLL4221());
            _model.Owner.cardSlotDetail.RecoverPlayPoint(_model.Owner.cardSlotDetail.GetMaxPlayPoint());
            if (egoOptions.EgoAbDialogList.Any())
                UnitUtil.BattleAbDialog(_model.Owner.view.dialogUI, egoOptions.EgoAbDialogList,
                    egoOptions.EgoAbColorColor);
            return true;
        }

        private static void ChangeToOmoriEgoAttackMap()
        {
            MapUtil.ChangeMap(new MapModel
            {
                Stage = "Omori2_Om21341",
                OriginalMapStageIds = new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) },
                IsPlayer = true,
                OneTurnEgo = true,
                Component = typeof(Omori2_Om21341MapManager),
                Bgy = 0.55f
            });
        }

        public override void ChangeToEgoMap(LorId cardId)
        {
            if (!_model.EgoMaps.ContainsKey(cardId) || _model.Owner.faction != Faction.Player ||
                SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.isEgo) return;
            _model.EgoMapAttackUsed = true;
            ChangeToOmoriEgoAttackMap();
        }

        public void ReturnFromEgoAttackMap()
        {
            if (!_model.EgoMapAttackUsed) return;
            _model.EgoMapAttackUsed = false;
            MapUtil.ReturnFromEgoMap("Omori2_Om21341", new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) });
        }

        public override void ReturnFromEgoMap()
        {
            if (!_model.MapChanged) return;
            _model.MapChanged = false;
            MapUtil.ReturnFromEgoMap("Omori5_Om21341", new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) },
                true);
        }
    }
}