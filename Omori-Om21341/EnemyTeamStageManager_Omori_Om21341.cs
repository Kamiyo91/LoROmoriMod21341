using System.Collections.Generic;
using System.Linq;
using BLL_Om21341.Models;
using BLL_Om21341.Models.Enum;
using BLL_Om21341.Models.MechUtilModels;
using EmotionalBurstPassive_Om21341.Passives;
using LOR_XML;
using Omori_Om21341.Buffs;
using Omori_Om21341.MapManagers;
using Omori_Om21341.MechUtil;
using Util_Om21341;
using Util_Om21341.CommonBuffs;
using Util_Om21341.CustomMapUtility.Assemblies;

namespace Omori_Om21341
{
    public class EnemyTeamStageManager_Omori_Om21341 : EnemyTeamStageManager
    {
        private int _linesCount;
        private NpcMechUtil_Omori _mechUtil;
        private bool _notSuccumb;
        private BattleUnitModel _omoriModel;
        public override void OnWaveStart()
        {
            CustomMapHandler.InitCustomMap("Omori1_Om21341", new Omori1_Om21341MapManager(), false, true, 0.5f, 0.55f);
            CustomMapHandler.InitCustomMap("Omori2_Om21341", new Omori2_Om21341MapManager(), false, true, 0.5f, 0.55f);
            CustomMapHandler.InitCustomMap("Omori3_Om21341", new Omori3_Om21341MapManager(), false, true, 0.5f, 0.55f);
            CustomMapHandler.InitCustomMap("Omori4_Om21341", new Omori4_Om21341MapManager(), false, true, 0.5f, 0.55f);
            CustomMapHandler.InitCustomMap("Omori5_Om21341", new Omori5_Om21341MapManager(), false, true, 0.5f, 0.55f);
            CustomMapHandler.InitCustomMap("Omori6_Om21341", new Omori6_Om21341MapManager(), false, true, 0.5f, 0.55f);
            CustomMapHandler.EnforceMap();
            _mechUtil = new NpcMechUtil_Omori(new NpcMechUtilBaseModel());
            Singleton<StageController>.Instance.CheckMapChange();
            _omoriModel = BattleObjectManager.instance.GetList(Faction.Enemy).FirstOrDefault();
            _omoriModel?.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_PlayerShimmeringBuf_Om21341());
            _omoriModel?.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_Immortal_Om21341());
            foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                unit.forceRetreat = true;
                PrepareUnitsPassives(unit);
                unit.SetHp((int)unit.passiveDetail.GetStartHp(unit.MaxHp));
            }
            _linesCount = 0;
            _notSuccumb = false;
        }

        private static void PrepareUnitsPassives(BattleUnitModel unit)
        {
            unit.passiveDetail.DestroyPassive(
                unit.passiveDetail.PassiveList.FirstOrDefault(x => x is PassiveAbility_EmotionalBurst_Om21341));
            switch (unit.UnitData.unitData.OwnerSephirah)
            {
                case SephirahType.Malkuth:
                    unit.passiveDetail.AddPassive(new LorId(ModParameters.PackageId, 46));
                    break;
                case SephirahType.Yesod:
                    unit.passiveDetail.AddPassive(new LorId(ModParameters.PackageId, 28));
                    break;
                case SephirahType.Hod:
                    unit.passiveDetail.AddPassive(new LorId(ModParameters.PackageId, 47));
                    break;
                case SephirahType.Netzach:
                    unit.passiveDetail.AddPassive(new LorId(ModParameters.PackageId, 48));
                    break;
            }
        }

        public override void OnRoundStart()
        {
            CustomMapHandler.EnforceMap(_notSuccumb ? _mechUtil.GetPhase() > 1 ? 5 : 4 : _mechUtil.GetPhase());
        }

        public override void OnRoundStart_After()
        {
            if (_mechUtil.GetPhase() > 0) OmoriShimmering();
            switch (_notSuccumb)
            {
                case false when _mechUtil.GetPhase() > 0:

                    UnitUtil.BattleAbDialog(_omoriModel.view.dialogUI,
                        new List<AbnormalityCardDialog>
                        {
                            new AbnormalityCardDialog
                            {
                                id = "Omori",
                                dialog =
                                    ModParameters.EffectTexts.ContainsKey(
                                        $"OmoriPhase{_mechUtil.GetPhase()}Line{_linesCount}_Om21341")
                                        ? ModParameters.EffectTexts.FirstOrDefault(x =>
                                                x.Key.Equals(
                                                    $"OmoriPhase{_mechUtil.GetPhase()}Line{_linesCount}_Om21341"))
                                            .Value.Desc
                                        : ModParameters.EffectTexts
                                            .FirstOrDefault(x => x.Key.Equals("OmoriFinalLine_Om21341")).Value.Desc
                            }
                        }, AbColorType.Negative);
                    _linesCount++;
                    break;
                case true:
                    UnitUtil.BattleAbDialog(_omoriModel.view.dialogUI,
                        new List<AbnormalityCardDialog>
                        {
                            new AbnormalityCardDialog
                            {
                                id = "Omori",
                                dialog = ModParameters.EffectTexts.FirstOrDefault(x =>
                                        x.Key.Equals($"OmoriSurvive{(_mechUtil.GetPhase() < 2 ? 1 : 2)}_Om21341"))
                                    .Value.Desc
                            }
                        }, AbColorType.Negative);
                    break;
            }
        }

        private void OmoriShimmering()
        {
            _omoriModel.allyCardDetail.ExhaustAllCards();
            _omoriModel.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 69));
            _omoriModel.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 72));
            _omoriModel.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 74));
            _omoriModel.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 75));
            _omoriModel.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 76));
            _omoriModel.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 76));
            _omoriModel.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 77));
            _omoriModel.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 77));
            _omoriModel.allyCardDetail.AddTempCard(new LorId(ModParameters.PackageId, 67));
        }

        public override void OnRoundEndTheLast()
        {
            CheckEndingCaseWin();
            CheckPhaseChange();
            _mechUtil.SetOneTurnCard(false);
        }

        public void CallMassAttack(ref BattleDiceCardModel origin)
        {
            _mechUtil.OnSelectCardPutMassAttack(ref origin);
        }

        public int GetPhase()
        {
            return _mechUtil.GetPhase();
        }

        private void CheckEndingCaseWin()
        {
            if (BattleObjectManager.instance.GetAliveList(Faction.Player).Count < 1 && _mechUtil.GetPhase() > 2)
                _omoriModel.DieFake();
        }

        private void CheckPhaseChange()
        {
            if (_omoriModel.IsDead() && _mechUtil.GetPhase() < 3) UnitUtil.UnitReviveAndRecovery(_omoriModel, 5, false);
            if (_notSuccumb)
            {
                _notSuccumb = false;
                if (_mechUtil.GetPhase() < 3)
                {
                    _mechUtil.IncreasePhase();
                    if (_mechUtil.GetPhase() == 3)
                        _omoriModel.passiveDetail.AddPassive(new LorId(ModParameters.PackageId, 54));
                    else
                        _linesCount = 0;
                }
                else
                {
                    BattleEnding();
                    return;
                }

                CustomMapHandler.EnforceMap(_mechUtil.GetPhase());
                Singleton<StageController>.Instance.CheckMapChange();
                UnitUtil.UnitReviveAndRecovery(_omoriModel, _omoriModel.MaxHp, true);
            }

            if (!(_omoriModel.hp < 2) || _notSuccumb || _mechUtil.GetPhase() >= 4) return;
            _notSuccumb = true;
            _omoriModel.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_UntargetableOmori_Om21341());
            CustomMapHandler.EnforceMap(_mechUtil.GetPhase() > 1 ? 5 : 4);
            Singleton<StageController>.Instance.CheckMapChange();
        }

        private void BattleEnding()
        {
            foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player)) unit.Die();
            _omoriModel.DieFake();
        }
    }
}