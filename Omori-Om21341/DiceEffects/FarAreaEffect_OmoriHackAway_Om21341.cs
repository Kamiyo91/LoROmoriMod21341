using System.Collections.Generic;
using System.Linq;
using LOR_XML;
using UnityEngine;

namespace Omori_Om21341.DiceEffects
{
    public class FarAreaEffect_OmoriHackAway_Om21341 : FarAreaEffect
    {
        private const float EndDelay = 0.175f;

        private const float MoveSpeed = 4.2f;

        private const float AtkDelay = 0.175f;

        private readonly Vector3 _offsetEnd = new Vector3(-12f, 0f, 0f);

        private readonly Vector3 _offsetStart = new Vector3(12f, 0f, 0f);

        private BattleFarAreaPlayManager.VictimInfo _currentVictim;

        private Vector3 _dstPosAtkOneTarget;

        private float _elapsedEndAction;

        private float _elapsedGiveDamage;

        private float _elapsedStart;

        private Vector3 _srcPosAtkOneTarget;

        private List<BattleFarAreaPlayManager.VictimInfo> _victimList;
        public override bool HasIndependentAction => true;

        public override void Init(BattleUnitModel self, params object[] args)
        {
            base.Init(self, args);
            state = EffectState.Start;
            _elapsedStart = 0f;
            _elapsedGiveDamage = 0f;
            _elapsedEndAction = 0f;
            _dstPosAtkOneTarget = Vector3.zero;
            _srcPosAtkOneTarget = Vector3.zero;
            isRunning = false;
            _currentVictim = null;
            Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay(0f, 0f);
            Singleton<BattleFarAreaPlayManager>.Instance.SetUIDelay(0f);
            Singleton<BattleFarAreaPlayManager>.Instance.SetRollDiceDelay(0.01f);
            Singleton<BattleFarAreaPlayManager>.Instance.SetPrintRollDiceDelay(0f);
        }

        public override bool ActionPhase(float deltaTime, BattleUnitModel attacker,
            List<BattleFarAreaPlayManager.VictimInfo> victims,
            ref List<BattleFarAreaPlayManager.VictimInfo> defenseVictims)
        {
            var result = false;
            switch (state)
            {
                case EffectState.Start:
                {
                    _elapsedStart += deltaTime;
                    if (!(_elapsedStart > 0.05f)) return false;
                    _elapsedStart = 0f;
                    state = EffectState.GiveDamage;
                    _victimList = new List<BattleFarAreaPlayManager.VictimInfo>(victims);
                    break;
                }
                case EffectState.GiveDamage:
                {
                    if (_elapsedGiveDamage < Mathf.Epsilon)
                    {
                        var victimList = _victimList;
                        if (victimList != null && victimList.Count > 0)
                            if (_victimList.Exists(x => !x.unitModel.IsDead()))
                            {
                                var list = _victimList.Where(victimInfo => !victimInfo.unitModel.IsDead()).ToList();
                                var victimInfo2 = list[Random.Range(0, list.Count)];
                                var num = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                                attacker.view.WorldPosition =
                                    victimInfo2.unitModel.view.WorldPosition + _offsetStart * num;
                                attacker.UpdateDirection(victimInfo2.unitModel.view.WorldPosition);
                                _srcPosAtkOneTarget = victimInfo2.unitModel.view.WorldPosition + _offsetStart * num;
                                _dstPosAtkOneTarget = victimInfo2.unitModel.view.WorldPosition + _offsetEnd * num;
                                victimInfo2.unitModel.UpdateDirection(attacker.view.WorldPosition);
                                _victimList.Remove(victimInfo2);
                                _currentVictim = victimInfo2;
                            }
                    }

                    _elapsedGiveDamage += deltaTime;
                    if (_currentVictim != null && _elapsedGiveDamage > AtkDelay)
                    {
                        var playingCard = _currentVictim.playingCard;
                        if (playingCard?.currentBehavior != null)
                        {
                            if (attacker.currentDiceAction.currentBehavior.DiceResultValue >
                                _currentVictim.playingCard.currentBehavior.DiceResultValue)
                            {
                                attacker.currentDiceAction.currentBehavior.GiveDamage(_currentVictim.unitModel);
                                if (_currentVictim.unitModel.IsDead())
                                {
                                    var list3 = new List<BattleUnitModel> { _self };
                                    _currentVictim.unitModel.view.DisplayDlg(DialogType.DEATH, list3);
                                }

                                _currentVictim.unitModel.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                                _currentVictim.destroyedDicesIndex.Add(_currentVictim.playingCard.currentBehavior
                                    .Index);
                            }
                            else
                            {
                                _currentVictim.unitModel.view.charAppearance.ChangeMotion(ActionDetail.Guard);
                                _currentVictim.unitModel.UpdateDirection(attacker.view.WorldPosition);
                                if (!defenseVictims.Contains(_currentVictim)) defenseVictims.Add(_currentVictim);
                            }
                        }
                        else
                        {
                            attacker.currentDiceAction.currentBehavior.GiveDamage(_currentVictim.unitModel);
                            if (_currentVictim.unitModel.IsDead())
                            {
                                var list4 = new List<BattleUnitModel>();
                                list4.Add(_self);
                                _currentVictim.unitModel.view.DisplayDlg(DialogType.DEATH, list4);
                            }

                            _currentVictim.unitModel.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                        }

                        var effectSrc = GetEffectSrc(RandomUtil.Range(0, 1));
                        SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect(effectSrc, 1f, _self.view,
                            _currentVictim.unitModel.view);
                        attacker.view.charAppearance.ChangeMotion(ActionDetail.Slash);
                        _self.view.charAppearance.soundInfo.PlaySound(
                            MotionConverter.ActionToMotion(ActionDetail.Slash),
                            true);
                        var x2 = Random.Range(0.04f, 0.08f);
                        var y = Random.Range(0.04f, 0.08f);
                        var speed = Random.Range(70f, 90f);
                        var time = Random.Range(0.1f, 0.15f);
                        CameraFilterUtil.EarthQuake(x2, y, speed, time);
                        SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(
                            _currentVictim.unitModel, _currentVictim.unitModel.faction, _currentVictim.unitModel.hp,
                            _currentVictim.unitModel.breakDetail.breakGauge);
                        SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(
                            attacker, attacker.faction, attacker.hp, attacker.breakDetail.breakGauge);
                        _currentVictim = null;
                    }

                    if (Vector3.SqrMagnitude(_dstPosAtkOneTarget - _srcPosAtkOneTarget) > Mathf.Epsilon &&
                        _elapsedGiveDamage > AtkDelay)
                        attacker.view.WorldPosition = Vector3.Lerp(_srcPosAtkOneTarget, _dstPosAtkOneTarget,
                            _elapsedGiveDamage * MoveSpeed);

                    if (!(_elapsedGiveDamage > EndDelay)) return false;
                    {
                        _elapsedGiveDamage = 0f;
                        _srcPosAtkOneTarget = Vector3.zero;
                        _dstPosAtkOneTarget = Vector3.zero;
                        if (_victimList == null || _victimList.Count == 0)
                        {
                            state = EffectState.End;
                        }
                        else if (!_victimList.Exists(x => !x.unitModel.IsDead()))
                        {
                            _victimList.Clear();
                            state = EffectState.End;
                        }
                    }
                    break;
                }
                case EffectState.End:
                {
                    _elapsedEndAction += deltaTime;
                    if (!(_elapsedEndAction > 0.01f)) return false;
                    _self.view.charAppearance.ChangeMotion(ActionDetail.Default);
                    state = EffectState.None;
                    _elapsedEndAction = 0f;
                    _isDoneEffect = true;
                    Destroy(gameObject);
                    break;
                }
                case EffectState.None:
                    break;
                default:
                {
                    if (_self.moveDetail.isArrived) result = true;

                    break;
                }
            }

            return result;
        }

        private static string GetEffectSrc(int number)
        {
            switch (number)
            {
                case 0:
                    return "BS4DurandalUp_J";
                default:
                    return "BS4DurandalDown_J2";
            }
        }
    }
}