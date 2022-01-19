using System.Collections.Generic;
using System.Linq;
using BLL_Om21341.Models;
using BLL_Om21341.Models.Enum;
using CustomMapUtility;
using EmotionalBurstPassive_Om21341.Passives;
using LOR_XML;
using Omori_Om21341.MapManagers;
using Omori_Om21341.Passives;
using UnityEngine;
using Util_Om21341;

namespace Omori_Om21341
{
    public class EnemyTeamStageManager_Omori_Om21341 : EnemyTeamStageManager
    {
        private int _linesCount;
        private BattleUnitModel _omoriModel;
        private PassiveAbility_OmoriNpc_Om21341 _omoriPassive;
        private List<BattleUnitModel> _playerUnits;
        public AudioClip LoopClip;
        public AudioSource Overlay;

        public override void OnWaveStart()
        {
            _playerUnits = new List<BattleUnitModel>();
            Overlay = Object.Instantiate(SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme,
                SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.transform.parent);
            Overlay.clip = null;
            Overlay.name = "overlay_OmoriOm21341";
            Overlay.loop = true;
            Overlay.Stop();
            CustomMapHandler.LoadEnemyTheme("boss_OMORI.ogg", out var introClip);
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
            CustomMapHandler.LoadEnemyTheme("b_omori_02.ogg");
            CustomMapHandler.LoadEnemyTheme("b_omori_03.ogg");
            CustomMapHandler.LoadEnemyTheme("b_omori_04.ogg");
            CustomMapHandler.EnforceMap();
            _omoriModel = BattleObjectManager.instance.GetList(Faction.Enemy).FirstOrDefault();
            _omoriPassive = _omoriModel
                ?.passiveDetail
                .PassiveList
                .Find(x => x is PassiveAbility_OmoriNpc_Om21341) as PassiveAbility_OmoriNpc_Om21341;
            Singleton<StageController>.Instance.CheckMapChange();
            foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
                PrepareUnitsPassives(unit);
            _linesCount = 0;
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
            CustomMapHandler.EnforceMap(_omoriPassive.GetSuccumbStatus() ? 4 : GetPhase());
        }

        public override void OnRoundStart_After()
        {
            switch (_omoriPassive.GetSuccumbStatus())
            {
                case false when _omoriPassive.GetMech().GetPhase() > 0:

                    UnitUtil.BattleAbDialog(_omoriModel.view.dialogUI,
                        new List<AbnormalityCardDialog>
                        {
                            new AbnormalityCardDialog
                            {
                                id = "Omori",
                                dialog =
                                    ModParameters.EffectTexts.ContainsKey(
                                        $"OmoriPhase{_omoriPassive.GetMech().GetPhase()}Line{_linesCount}_Om21341")
                                        ? ModParameters.EffectTexts.FirstOrDefault(x =>
                                                x.Key.Equals(
                                                    $"OmoriPhase{_omoriPassive.GetMech().GetPhase()}Line{_linesCount}_Om21341"))
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
                                        x.Key.Equals(
                                            $"OmoriSurvive{(_omoriPassive.GetMech().GetPhase() < 2 ? 1 : 2)}_Om21341"))
                                    .Value.Desc
                            }
                        }, AbColorType.Negative);
                    break;
            }
        }

        private int GetPhase()
        {
            return _omoriPassive.GetMech().GetPhase();
        }

        public void SetLinesTo0()
        {
            _linesCount = 0;
        }

        public void SetOverlay(int phase)
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

        public override void OnEndBattle()
        {
            foreach (var unit in _playerUnits) unit.Revive(1);
            MapUtil.UnloadBoomEffect();
        }
    }
}