using System.Collections.Generic;
using System.Linq;
using BLL_Om21341.Models;
using BLL_Om21341.Models.Enum;
using BLL_Om21341.Models.MechUtilModels;
using CustomMapUtility;
using EmotionalBurstPassive_Om21341.Passives;
using LOR_XML;
using Omori_Om21341.Buffs;
using Omori_Om21341.MapManagers;
using Omori_Om21341.MechUtil;
using UnityEngine;
using Util_Om21341;
using Util_Om21341.CommonBuffs;

namespace Omori_Om21341
{
    public class EnemyTeamStageManager_Omori_Om21341 : EnemyTeamStageManager
    {
        private int _linesCount;
        private NpcMechUtil_Omori _mechUtil;
        private bool _notSuccumb;
        private BattleUnitModel _omoriModel;
        private List<BattleUnitModel> _playerUnits;
        public AudioClip LoopClip;
        public AudioSource Overlay;

        public override void OnWaveStart()
        {
            _playerUnits = new List<BattleUnitModel>();
            Overlay = Object.Instantiate(SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme, SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.transform.parent);
            Overlay.clip = null;
            Overlay.name = "overlay_OmoriOm21341";
            Overlay.loop = true;
            Overlay.Stop();
            CustomMapHandler.LoadEnemyTheme("boss_OMORI.ogg", out var introClip);
            // CustomMapHandler.LoadEnemyTheme("boss_OMORI_loop.ogg");
            LoopClip = CustomMapHandler.ClipCut(introClip, 1860207, 9305332, "boss_OMORI_loop");
            CustomMapHandler.InitCustomMap("Omori1_Om21341", typeof(Omori1_Om21341MapManager), false, true, 0.5f,
                0.55f);
            CustomMapHandler.InitCustomMap("Omori2_Om21341", typeof(Omori2_Om21341MapManager), false, false, 0.5f,
                0.55f);
            CustomMapHandler.InitCustomMap("Omori3_Om21341", typeof(Omori3_Om21341MapManager), false, false, 0.5f,
                0.55f);
            CustomMapHandler.InitCustomMap("Omori4_Om21341", typeof(Omori4_Om21341MapManager), false, false, 0.5f,
                0.55f);
            CustomMapHandler.InitCustomMap("Omori5_Om21341", typeof(Omori5_Om21341MapManager), false, false, 0.5f,
                0.55f);
            // CustomMapHandler.LoadEnemyTheme("b_omori_01.ogg");
            CustomMapHandler.LoadEnemyTheme("b_omori_02.ogg");
            CustomMapHandler.LoadEnemyTheme("b_omori_03.ogg");
            CustomMapHandler.LoadEnemyTheme("b_omori_04.ogg");
            CustomMapHandler.EnforceMap();
            _mechUtil = new NpcMechUtil_Omori(new NpcMechUtilBaseModel());
            Singleton<StageController>.Instance.CheckMapChange();
            _omoriModel = BattleObjectManager.instance.GetList(Faction.Enemy).FirstOrDefault();
            _omoriModel?.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_PlayerShimmeringBuf_Om21341());
            _omoriModel?.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_Immortal_Om21341());
            foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
                PrepareUnitsPassives(unit);
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

        public void AddUnitToReviveList(BattleUnitModel unit)
        {
            _playerUnits.Add(unit);
        }

        public override void OnRoundStart()
        {
            CustomMapHandler.EnforceMap(_notSuccumb ? 4 : _mechUtil.GetPhase());
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
                    SetOverlay(_mechUtil.GetPhase());
                    if (_mechUtil.GetPhase() == 3)
                    {
                        _omoriModel.passiveDetail.AddPassive(new LorId(ModParameters.PackageId, 54));
                        foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
                            unit.forceRetreat = true;
                    }
                    else
                    {
                        _linesCount = 0;
                    }
                }
                else
                {
                    BattleEnding();
                    return;
                }

                UnitUtil.UnitReviveAndRecovery(_omoriModel, _omoriModel.MaxHp, true);
            }

            if (!(_omoriModel.hp < 2) || _notSuccumb || _mechUtil.GetPhase() >= 4) return;
            _notSuccumb = true;
            _omoriModel.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_UntargetableOmori_Om21341());
        }

        private void SetOverlay(int phase)
        {
            Overlay.volume = SingletonBehavior<BattleSoundManager>.Instance.VolumeBGM;
            switch (phase)
            {
                case 1:
                    Overlay.clip = CustomMapHandler.GetAudioClip("b_omori_02.ogg");
                    Overlay.Play();
                    break;
                case 2:
                    Overlay.clip = CustomMapHandler.GetAudioClip("b_omori_03.ogg");
                    Overlay.Play();
                    break;
                case 3:
                    Overlay.clip = CustomMapHandler.GetAudioClip("b_omori_04.ogg");
                    Overlay.Play();
                    break;
            }
        }

        private void BattleEnding()
        {
            foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                _playerUnits.Add(unit);
                unit.Die();
            }

            _omoriModel.DieFake();
        }

        public override void OnEndBattle()
        {
            foreach (var unit in _playerUnits) unit.Revive(1);
            MapUtil.UnloadBoomEffect();
        }
    }
}