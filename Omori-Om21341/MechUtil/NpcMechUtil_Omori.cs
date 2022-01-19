using BLL_Om21341.Extensions.MechUtilModelExtensions;
using BLL_Om21341.Models;
using Omori_Om21341.Buffs;
using Omori_Om21341.MapManagers;
using Util_Om21341;
using Util_Om21341.BaseClass;

namespace Omori_Om21341.MechUtil
{
    public class NpcMechUtil_Omori : NpcMechUtilBase
    {
        private readonly NpcMechUtil_OmoriModel _model;
        private EnemyTeamStageManager_Omori_Om21341 _stageManager;

        public NpcMechUtil_Omori(NpcMechUtil_OmoriModel model) : base(model)
        {
            _model = model;
        }

        public int GetPhase()
        {
            return _model.Phase;
        }

        private void IncreasePhase()
        {
            _model.Phase++;
        }

        public void SetStageManager(EnemyTeamStageManager_Omori_Om21341 manager)
        {
            _stageManager = manager;
        }

        public EnemyTeamStageManager_Omori_Om21341 GetStageManager()
        {
            return _stageManager;
        }

        public override void OnSelectCardPutMassAttack(ref BattleDiceCardModel origin)
        {
            if (GetPhase() == 1 && !_model.SingleUse)
            {
                _model.SingleUse = true;
                origin = BattleDiceCardModel.CreatePlayingCard(
                    ItemXmlDataList.instance.GetCardItem(new LorId(ModParameters.PackageId, 907)));
                return;
            }

            if (GetPhase() <= 1 || _model.OneTurnCard) return;
            SetOneTurnCard(true);
            origin = BattleDiceCardModel.CreatePlayingCard(
                ItemXmlDataList.instance.GetCardItem(new LorId(ModParameters.PackageId, 907)));
        }

        private static void ChangeToOmoriEgoMap()
        {
            MapUtil.ChangeMap(new MapModel
            {
                Stage = "Omori5_Om21341",
                StageId = 8,
                IsPlayer = true,
                Component = typeof(Omori5_Om21341MapManager),
                Bgy = 0.55f
            });
        }

        private static void ChangeToOmoriEgoAttackMap()
        {
            MapUtil.ChangeMap(new MapModel
            {
                Stage = "Omori2_Om21341",
                StageId = 8,
                IsPlayer = true,
                OneTurnEgo = true,
                Component = typeof(Omori2_Om21341MapManager),
                Bgy = 0.55f
            });
        }

        public void ChangeMap()
        {
            if (!_model.NotSuccumb) _model.NotSuccumb = false;
            if (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.isEgo) return;
            _model.MapChanged = true;
            ChangeToOmoriEgoMap();
        }

        public override void SurviveCheck(int dmg)
        {
            if (_model.Owner.hp - dmg > 1 || _model.NotSuccumb) return;
            _model.NotSuccumb = true;
        }

        public virtual void ChangeToEgoMap(LorId cardId)
        {
            if (cardId != new LorId(ModParameters.PackageId, 907) ||
                SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.isEgo) return;
            _model.AttackMapChanged = true;
            ChangeToOmoriEgoAttackMap();
        }

        public void ReturnFromEgoAttackMap()
        {
            if (!_model.AttackMapChanged) return;
            _model.AttackMapChanged = false;
            MapUtil.ReturnFromEgoMap("Omori2_Om21341", 8);
        }

        public void ReturnFromEgoMap()
        {
            if (!_model.MapChanged) return;
            _model.MapChanged = false;
            MapUtil.ReturnFromEgoMap("Omori5_Om21341", 8, true);
        }

        public void CheckPhaseChange()
        {
            if (_model.Owner.IsDead() && GetPhase() < 3) UnitUtil.UnitReviveAndRecovery(_model.Owner, 5, false);
            if (_model.NotSuccumbMech)
            {
                _model.NotSuccumbMech = false;
                if (GetPhase() < 3)
                {
                    IncreasePhase();
                    _stageManager?.SetOverlay(GetPhase());
                    if (GetPhase() == 3)
                    {
                        _model.Owner.passiveDetail.AddPassive(new LorId(ModParameters.PackageId, 54));
                        foreach (var unit in BattleObjectManager.instance.GetAliveList(
                                     UnitUtil.ReturnOtherSideFaction(_model.Owner.faction)))
                            unit.forceRetreat = true;
                    }
                    else
                    {
                        _stageManager?.SetLinesTo0();
                    }
                }
                else
                {
                    BattleEnding();
                    return;
                }

                UnitUtil.UnitReviveAndRecovery(_model.Owner, _model.Owner.MaxHp, true);
            }

            if (!(_model.Owner.hp < 2) || _model.NotSuccumbMech || GetPhase() >= 4) return;
            _model.NotSuccumbMech = true;
            _model.Owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_UntargetableOmori_Om21341());
        }

        private void BattleEnding()
        {
            foreach (var unit in BattleObjectManager.instance.GetAliveList(
                         UnitUtil.ReturnOtherSideFaction(_model.Owner.faction)))
            {
                _stageManager?.AddUnitToReviveList(unit);
                unit.Die();
            }

            _model.Owner.DieFake();
        }

        public void OmoriShimmering()
        {
            _model.Owner.allyCardDetail.ExhaustAllCards();
            _model.Owner.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 69));
            _model.Owner.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 72));
            _model.Owner.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 74));
            _model.Owner.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 75));
            _model.Owner.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 76));
            _model.Owner.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 76));
            _model.Owner.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 77));
            _model.Owner.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 77));
            _model.Owner.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 67));
        }

        public void CheckEndingCaseWin()
        {
            if (BattleObjectManager.instance.GetAliveList(UnitUtil.ReturnOtherSideFaction(_model.Owner.faction)).Count <
                1 && GetPhase() > 2)
                _model.Owner.DieFake();
        }

        public bool GetSuccumbMechStatus()
        {
            return _model.NotSuccumbMech;
        }
    }
}