using System.Collections.Generic;
using System.Linq;
using CustomMapUtility;
using LOR_XML;
using OmoriMod_Om21341.BLL_Om21341;
using OmoriMod_Om21341.BLL_Om21341.Enum;
using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Cards;
using OmoriMod_Om21341.Omori_Om21341.Buffs;
using OmoriMod_Om21341.Omori_Om21341.MapManagers;
using OmoriMod_Om21341.Util_Om21341;
using UnityEngine;
using UtilLoader21341;
using UtilLoader21341.Extensions;
using UtilLoader21341.Util;

namespace OmoriMod_Om21341.Omori_Om21341.Passives
{
    public class PassiveAbility_OmoriNpc_Om21341 : PassiveAbilityBase
    {
        private readonly CustomMapHandler _cmh = CustomMapHandler.GetCMU(OmoriModParameters.PackageId);

        private readonly List<EmotionBufEnum> _emotionBufEnum = new List<EmotionBufEnum>
            { EmotionBufEnum.Neutral, EmotionBufEnum.Angry, EmotionBufEnum.Happy, EmotionBufEnum.Sad };

        private bool _attackMapUsed;
        private int _linesCount;
        private bool _machChanging;
        private bool _mapActive;
        private bool _mapChanged;
        private BattleUnitModel _omoriModel;
        private bool _oneSceneCard;
        private int _phase;
        private List<BattleUnitModel> _playerUnits;
        private bool _singleUse;
        private bool _succumbStatus;
        public AudioClip LoopClip;
        public AudioSource Overlay;

        public override void OnWaveStart()
        {
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_AfraidImmunity_Om21341());
            if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id.packageId ==
                OmoriModParameters.PackageId &&
                Singleton<StageController>.Instance.GetStageModel().ClassInfo.id.id == 8) OnWaveStartMaps();
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_ChangeCardCost_DLL21341());
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_Immortal_DLL21341(false, infinite: true,
                lastOneScene: false, canRecoverHp: true, canRecoverBp: true));
        }

        public void OnWaveStartMaps()
        {
            _mapActive = true;
            MapUtil.InitEnemyMap<Omori1_Om21341MapManager>(_cmh, OmoriModParameters.OmoriMap1);
            MapUtil.InitEnemyMap<Omori2_Om21341MapManager>(_cmh, OmoriModParameters.OmoriMap2);
            MapUtil.InitEnemyMap<Omori3_Om21341MapManager>(_cmh, OmoriModParameters.OmoriMap3);
            MapUtil.InitEnemyMap<Omori4_Om21341MapManager>(_cmh, OmoriModParameters.OmoriMap4);
            MapUtil.InitEnemyMap<Omori5_Om21341MapManager>(_cmh, OmoriModParameters.OmoriMap5);
            _cmh.EnforceMap();
            _playerUnits = new List<BattleUnitModel>();
            Overlay = Object.Instantiate(SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme,
                SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.transform.parent);
            Overlay.clip = null;
            Overlay.name = "overlay_OmoriOm21341";
            Overlay.loop = true;
            Overlay.Stop();
            _cmh.LoadEnemyTheme("boss_OMORI.ogg", out var introClip);
            LoopClip = _cmh.ClipCut(introClip, 1860207, 9305332, "boss_OMORI_loop");
            _cmh.LoadEnemyTheme("b_omori_02.ogg");
            _cmh.LoadEnemyTheme("b_omori_03.ogg");
            _cmh.LoadEnemyTheme("b_omori_04.ogg");
            _omoriModel = BattleObjectManager.instance.GetList(Faction.Enemy).FirstOrDefault();
            Singleton<StageController>.Instance.CheckMapChange();
            foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
                OmoriUtil.PrepareUnitsPassives(unit);
            _linesCount = 0;
        }

        public override void OnRoundStartAfter()
        {
            ChangeMap();
            if (_phase > 0) OmoriShimmering();
            var randomEmotion = RandomUtil.SelectOne(_emotionBufEnum);
            switch (randomEmotion)
            {
                case EmotionBufEnum.Neutral:
                    DiceCardSelfAbility_Neutral_Om21341.Activate(owner);
                    break;
                case EmotionBufEnum.Sad:
                    DiceCardSelfAbility_Sad_Om21341.Activate(owner);
                    break;
                case EmotionBufEnum.Angry:
                    DiceCardSelfAbility_Angry_Om21341.Activate(owner);
                    break;
                case EmotionBufEnum.Happy:
                    DiceCardSelfAbility_Happy_Om21341.Activate(owner);
                    break;
                case EmotionBufEnum.All:
                case EmotionBufEnum.Afraid:
                default:
                    DiceCardSelfAbility_Neutral_Om21341.Activate(owner);
                    break;
            }
        }

        public void ChangeMap()
        {
            if (!_mapActive) return;
            _cmh.EnforceMap(_succumbStatus ? 4 : _phase);
            ShowOmoriLine();
        }

        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (_phase <= 2) return;
            _playerUnits.Add(unit);
        }

        public override void AfterTakeDamage(BattleUnitModel attacker, int dmg)
        {
            owner.MechHpCheck(dmg, 1, ref _machChanging);
            if (_machChanging) owner.breakDetail.LoseBreakLife(attacker);
        }

        public override int GetMaxHpBonus()
        {
            switch (_phase)
            {
                case 0:
                    return 0;
                case 1:
                    return 35;
                case 2:
                    return 75;
                default:
                    return 150;
            }
        }

        public override int SpeedDiceNumAdder()
        {
            return _phase + 2;
        }

        public override BattleDiceCardModel OnSelectCardAuto(BattleDiceCardModel origin, int currentDiceSlotIdx)
        {
            OnSelectCardPutMassAttack(ref origin);
            return base.OnSelectCardAuto(origin, currentDiceSlotIdx);
        }

        public void OnSelectCardPutMassAttack(ref BattleDiceCardModel origin)
        {
            if (_phase == 1 && !_singleUse)
            {
                _singleUse = true;
                origin = BattleDiceCardModel.CreatePlayingCard(
                    ItemXmlDataList.instance.GetCardItem(new LorId(OmoriModParameters.PackageId, 907)));
                return;
            }

            if (_phase <= 1 || _oneSceneCard) return;
            _oneSceneCard = true;
            origin = BattleDiceCardModel.CreatePlayingCard(
                ItemXmlDataList.instance.GetCardItem(new LorId(OmoriModParameters.PackageId, 907)));
        }

        public override void OnRoundEndTheLast_ignoreDead()
        {
            OmoriUtil.ReturnFromEgoMap(ref _mapChanged);
            OmoriUtil.ReturnFromEgoAttackMap(ref _attackMapUsed);
            CheckEndingCaseWin();
            CheckPhaseChange();
            _oneSceneCard = false;
        }

        public void CheckEndingCaseWin()
        {
            if (BattleObjectManager.instance.GetAliveList(owner.faction.ReturnOtherSideFaction()).Count <
                1 && _phase > 2)
                owner.DieFake();
        }

        public void OmoriShimmering()
        {
            owner.allyCardDetail.ExhaustAllCards();
            owner.allyCardDetail.AddTempCard(new LorId(OmoriModParameters.PackageId, 69));
            owner.allyCardDetail.AddTempCard(new LorId(OmoriModParameters.PackageId, 72));
            owner.allyCardDetail.AddTempCard(new LorId(OmoriModParameters.PackageId, 74));
            owner.allyCardDetail.AddTempCard(new LorId(OmoriModParameters.PackageId, 75));
            owner.allyCardDetail.AddTempCard(new LorId(OmoriModParameters.PackageId, 76));
            owner.allyCardDetail.AddTempCard(new LorId(OmoriModParameters.PackageId, 76));
            owner.allyCardDetail.AddTempCard(new LorId(OmoriModParameters.PackageId, 77));
            owner.allyCardDetail.AddTempCard(new LorId(OmoriModParameters.PackageId, 77));
            owner.allyCardDetail.AddTempCard(new LorId(OmoriModParameters.PackageId, 67));
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            OmoriUtil.ChangeToEgoMap(curCard.card.GetID(), ref _attackMapUsed);
        }

        public void ShowOmoriLine()
        {
            switch (_succumbStatus)
            {
                case false when _phase > 0:

                    UnitUtil.BattleAbDialog(_omoriModel.view.dialogUI,
                        new List<AbnormalityCardDialog>
                        {
                            new AbnormalityCardDialog
                            {
                                id = "Omori",
                                dialog =
                                    ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts.ContainsKey(
                                        $"OmoriPhase{_phase}Line{_linesCount}_Om21341")
                                        ? ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                                            .FirstOrDefault(x =>
                                                x.Key.Equals(
                                                    $"OmoriPhase{_phase}Line{_linesCount}_Om21341"))
                                            .Value.Desc
                                        : ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                                            .FirstOrDefault(x => x.Key.Equals("OmoriFinalLine_Om21341")).Value.Desc
                            }
                        }, new Color(0.5f, 0, 0, 1f));
                    _linesCount++;
                    break;
                case true:
                    UnitUtil.BattleAbDialog(_omoriModel.view.dialogUI,
                        new List<AbnormalityCardDialog>
                        {
                            new AbnormalityCardDialog
                            {
                                id = "Omori",
                                dialog = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                                    .FirstOrDefault(x =>
                                        x.Key.Equals(
                                            $"OmoriSurvive{(_phase < 2 ? 1 : 2)}_Om21341"))
                                    .Value.Desc
                            }
                        }, new Color(0.5f, 0, 0, 1f));
                    break;
            }
        }

        public void SetOverlay(int phase)
        {
            Overlay.volume = SingletonBehavior<BattleSoundManager>.Instance.VolumeBGM;
            switch (phase)
            {
                case 1:
                    Overlay.clip = _cmh.GetAudioClip("b_omori_02.ogg");
                    Overlay.Play();
                    break;
                case 2:
                    Overlay.clip = _cmh.GetAudioClip("b_omori_03.ogg");
                    Overlay.Play();
                    break;
                case 3:
                    Overlay.clip = _cmh.GetAudioClip("b_omori_04.ogg");
                    Overlay.Play();
                    break;
            }
        }

        public override void OnBattleEnd()
        {
            Overlay.Stop();
            Object.Destroy(Overlay);
            foreach (var unit in _playerUnits) unit.Revive(1);
            CommonUtil.UnloadBoomEffect();
        }

        public void CheckPhaseChange()
        {
            if (owner.IsDead() && _phase < 3) owner.UnitReviveAndRecovery(5, false);
            if (_succumbStatus)
            {
                OmoriUtil.ChangeMapToSuccumbState(ref _succumbStatus, ref _mapChanged);
                if (_phase < 3)
                {
                    _phase++;
                    SetOverlay(_phase);
                    if (_phase == 3)
                    {
                        owner.passiveDetail.AddPassive(new LorId(OmoriModParameters.PackageId, 54));
                        foreach (var unit in BattleObjectManager.instance.GetAliveList(
                                     owner.faction.ReturnOtherSideFaction()))
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

                owner.UnitReviveAndRecovery(owner.MaxHp, true);
            }

            if (!(owner.hp < 2) || _succumbStatus || _phase >= 4) return;
            _succumbStatus = true;
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_UntargetableOmori_Om21341());
        }

        private void BattleEnding()
        {
            owner.DieFake();
            foreach (var unit in BattleObjectManager.instance.GetAliveList(
                         owner.faction.ReturnOtherSideFaction()))
            {
                _playerUnits.Add(unit);
                unit.Die();
            }
        }
    }
}