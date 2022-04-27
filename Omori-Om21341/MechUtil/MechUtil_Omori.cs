using System;
using System.Collections.Generic;
using BLL_Om21341;
using BLL_Om21341.Extensions.MechUtilModelExtensions;
using KamiyoStaticBLL.Models;
using KamiyoStaticUtil.BaseClass;
using KamiyoStaticUtil.CommonBuffs;
using KamiyoStaticUtil.Utils;
using Omori_Om21341.MapManagers;
using Util_Om21341;

namespace Omori_Om21341.MechUtil
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
            if (_model.Owner.hp - dmg > _model.Hp || !_model.Survive) return;
            _model.Survive = false;
            _model.RechargeCount = 0;
            _model.Owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_KamiyoImmortalUntilRoundEnd());
            UnitUtil.UnitReviveAndRecovery(_model.Owner, 0, _model.RecoverLightOnSurvive);
            _model.Owner.SetHp(_model.SetHp);
            _model.Owner.bufListDetail.AddBufWithoutDuplication(
                new BattleUnitBuf_KamiyoImmunityToStatusAlimentUntilRoundEnd());
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
                StageIds = new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) },
                IsPlayer = true,
                Component = typeof(Omori5_Om21341MapManager),
                Bgy = 0.55f
            });
        }

        public override void EgoActive()
        {
            _model.Owner.bufListDetail.AddBufWithoutDuplication(
                (BattleUnitBuf)Activator.CreateInstance(_model.EgoType));
            _model.Owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_KamiyoImmortalUntilRoundEnd());
            _model.Owner.cardSlotDetail.RecoverPlayPoint(_model.Owner.cardSlotDetail.GetMaxPlayPoint());
            if (_model.HasEgoAbDialog)
                UnitUtil.BattleAbDialog(_model.Owner.view.dialogUI, _model.EgoAbDialogList, _model.EgoAbColorColor);
        }

        private static void ChangeToOmoriEgoAttackMap()
        {
            MapUtil.ChangeMap(new MapModel
            {
                Stage = "Omori2_Om21341",
                StageIds = new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) },
                IsPlayer = true,
                OneTurnEgo = true,
                Component = typeof(Omori2_Om21341MapManager),
                Bgy = 0.55f
            });
        }

        public virtual void ChangeToEgoMap(LorId cardId)
        {
            if (cardId != _model.EgoAttackCardId || _model.Owner.faction != Faction.Player ||
                SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.isEgo) return;
            _model.EgoMapAttackused = true;
            ChangeToOmoriEgoAttackMap();
        }

        public void ReturnFromEgoAttackMap()
        {
            if (!_model.EgoMapAttackused) return;
            _model.EgoMapAttackused = false;
            MapUtil.ReturnFromEgoMap("Omori2_Om21341", new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) });
        }

        public void ReturnFromEgoMap()
        {
            if (!_model.MapChanged) return;
            _model.MapChanged = false;
            MapUtil.ReturnFromEgoMap("Omori5_Om21341", new List<LorId> { new LorId(OmoriModParameters.PackageId, 8) },
                true);
        }
    }
}