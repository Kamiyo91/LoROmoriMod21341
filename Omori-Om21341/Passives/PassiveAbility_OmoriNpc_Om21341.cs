using System.Collections.Generic;
using BigDLL4221.Buffs;
using OmoriMod_Om21341.BLL_Om21341.Enum;
using OmoriMod_Om21341.BLL_Om21341.Extensions.MechUtilModelExtensions;
using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Cards;
using OmoriMod_Om21341.Omori_Om21341.Buffs;
using OmoriMod_Om21341.Omori_Om21341.MechUtil;

namespace OmoriMod_Om21341.Omori_Om21341.Passives
{
    public class PassiveAbility_OmoriNpc_Om21341 : PassiveAbilityBase
    {
        private readonly List<EmotionBufEnum> _emotionBufEnum = new List<EmotionBufEnum>
            { EmotionBufEnum.Neutral, EmotionBufEnum.Angry, EmotionBufEnum.Happy, EmotionBufEnum.Sad };

        private NpcMechUtil_Omori _mechUtil;

        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            _mechUtil = new NpcMechUtil_Omori(new NpcMechUtil_OmoriModel("PhaseOmoriOm21341") { Owner = self });
        }

        public override void OnWaveStart()
        {
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_AfraidImmunity_Om21341());
            if (Singleton<StageController>.Instance.EnemyStageManager is EnemyTeamStageManager_Omori_Om21341 manager)
                _mechUtil.SetStageManager(manager);
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_ChangeCardCost_DLL4221());
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_Immortal_DLL4221(false, infinite: true,
                lastOneScene: false, canRecoverHp: true, canRecoverBp: true));
        }

        public override void OnRoundStartAfter()
        {
            _mechUtil.ChangeMap();
            if (_mechUtil.GetPhase() > 0) _mechUtil.OmoriShimmering();
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

        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (_mechUtil.GetPhase() > 2) _mechUtil.GetStageManager()?.AddUnitToReviveList(unit);
        }

        public override void AfterTakeDamage(BattleUnitModel attacker, int dmg)
        {
            _mechUtil.SurviveCheck(dmg);
            if (owner.hp < 2)
                owner.breakDetail.LoseBreakLife(attacker);
        }

        public override int GetMaxHpBonus()
        {
            switch (_mechUtil.GetPhase())
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

        public NpcMechUtil_Omori GetMech()
        {
            return _mechUtil;
        }

        public bool GetSuccumbStatus()
        {
            return _mechUtil.GetSuccumbMechStatus();
        }

        public override int SpeedDiceNumAdder()
        {
            return _mechUtil.GetPhase() + 2;
        }

        public override BattleDiceCardModel OnSelectCardAuto(BattleDiceCardModel origin, int currentDiceSlotIdx)
        {
            _mechUtil.OnSelectCardPutMassAttack(ref origin);
            return base.OnSelectCardAuto(origin, currentDiceSlotIdx);
        }

        public override void OnRoundEndTheLast_ignoreDead()
        {
            _mechUtil.ReturnFromEgoMap();
            _mechUtil.ReturnFromEgoAttackMap();
            _mechUtil.CheckEndingCaseWin();
            _mechUtil.CheckPhaseChange();
            _mechUtil.SetOneTurnCard(false);
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            _mechUtil.ChangeToEgoMap(curCard.card.GetID());
        }
    }
}