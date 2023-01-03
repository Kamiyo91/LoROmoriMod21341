using System.Collections.Generic;
using System.Linq;
using BigDLL4221.Enum;
using BigDLL4221.Models;
using BigDLL4221.StageManagers;
using BigDLL4221.Utils;
using CustomMapUtility;
using LOR_XML;
using OmoriMod_Om21341.BLL_Om21341;
using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Passives;
using OmoriMod_Om21341.Omori_Om21341.Passives;
using OmoriMod_Om21341.Util_Om21341;
using UnityEngine;

namespace OmoriMod_Om21341.Omori_Om21341
{
#pragma warning disable
    public class EnemyTeamStageManager_Omori_Om21341 : EnemyTeamStageManager_BaseWithCMUOnly_DLL4221
    {
        private readonly CustomMapHandler _cmh = CustomMapHandler.GetCMU(OmoriModParameters.PackageId);
        private int _linesCount;
        private BattleUnitModel _omoriModel;
        private PassiveAbility_OmoriNpc_Om21341 _omoriPassive;
        private List<BattleUnitModel> _playerUnits;
        public AudioClip LoopClip;
        public AudioSource Overlay;

        public override void OnWaveStart()
        {
            SetParameters(_cmh,
                new List<MapModel>
                {
                    OmoriModParameters.OmoriMap1, OmoriModParameters.OmoriMap2, OmoriModParameters.OmoriMap3,
                    OmoriModParameters.OmoriMap4, OmoriModParameters.OmoriMap5
                });
            _playerUnits = new List<BattleUnitModel>();
            Overlay = Object.Instantiate(SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme,
                SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.transform.parent);
            Overlay.clip = null;
            Overlay.name = "overlay_OmoriOm21341";
            Overlay.loop = true;
            Overlay.Stop();
            _cmh.LoadEnemyTheme("boss_OMORI.ogg", out var introClip);
            LoopClip = _cmh.ClipCut(introClip, 1860207, 9305332, "boss_OMORI_loop");
            base.OnWaveStart();
            _cmh.LoadEnemyTheme("b_omori_02.ogg");
            _cmh.LoadEnemyTheme("b_omori_03.ogg");
            _cmh.LoadEnemyTheme("b_omori_04.ogg");
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

        public void AddUnitToReviveList(BattleUnitModel unit)
        {
            _playerUnits.Add(unit);
        }

        public override void OnRoundStart()
        {
            ChangePhase(_omoriPassive.GetSuccumbStatus() ? 4 : GetPhase());
            base.OnRoundStart();
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
                                    ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts.ContainsKey(
                                        $"OmoriPhase{_omoriPassive.GetMech().GetPhase()}Line{_linesCount}_Om21341")
                                        ? ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                                            .FirstOrDefault(x =>
                                                x.Key.Equals(
                                                    $"OmoriPhase{_omoriPassive.GetMech().GetPhase()}Line{_linesCount}_Om21341"))
                                            .Value.Desc
                                        : ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
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
                                dialog = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                                    .FirstOrDefault(x =>
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

        public override void OnEndBattle()
        {
            foreach (var unit in _playerUnits) unit.Revive(1);
            CommonUtil.UnloadBoomEffect();
        }
    }
}