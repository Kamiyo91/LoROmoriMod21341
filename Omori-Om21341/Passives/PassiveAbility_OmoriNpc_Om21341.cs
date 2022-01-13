using System.Collections.Generic;
using BLL_Om21341.Models.Enum;
using EmotionalBurstPassive_Om21341.Cards;
using Omori_Om21341.Buffs;

namespace Omori_Om21341.Passives
{
    public class PassiveAbility_OmoriNpc_Om21341 : PassiveAbilityBase
    {
        private readonly List<EmotionBufEnum> _emotionBufEnum = new List<EmotionBufEnum>
            { EmotionBufEnum.Neutral, EmotionBufEnum.Angry, EmotionBufEnum.Happy, EmotionBufEnum.Sad };

        private EnemyTeamStageManager_Omori_Om21341 _stageManager;

        public override void OnWaveStart()
        {
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_AfraidImmunity_Om21341());
            if (Singleton<StageController>.Instance.EnemyStageManager is EnemyTeamStageManager_Omori_Om21341 manager)
                _stageManager = manager;
        }

        public override void OnRoundStartAfter()
        {
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
            if (_stageManager.GetPhase() > 2) _stageManager.AddUnitToReviveList(unit);
        }

        public override void AfterTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (owner.hp < 2)
                owner.breakDetail.LoseBreakLife(attacker);
        }

        public override int GetMaxHpBonus()
        {
            if (_stageManager == null) return 0;
            switch (_stageManager.GetPhase())
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
            return _stageManager.GetPhase() + 2;
        }

        public override BattleDiceCardModel OnSelectCardAuto(BattleDiceCardModel origin, int currentDiceSlotIdx)
        {
            _stageManager?.CallMassAttack(ref origin);
            return base.OnSelectCardAuto(origin, currentDiceSlotIdx);
        }
    }
}