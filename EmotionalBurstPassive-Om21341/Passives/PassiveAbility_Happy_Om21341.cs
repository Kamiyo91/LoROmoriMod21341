using System;
using System.Linq;
using EmotionalBurstPassive_Om21341.Buffs;
using KamiyoStaticBLL.Models;
using KamiyoStaticUtil.Utils;

namespace EmotionalBurstPassive_Om21341.Passives
{
    public class PassiveAbility_Happy_Om21341 : PassiveAbilityBase
    {
        private static readonly Random RndChance = new Random();
        private int _stack;

        public int GetStack()
        {
            return _stack;
        }

        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            Hide();
        }

        public void ChangeNameAndSetStacks(int stack)
        {
            switch (stack)
            {
                case 1:
                    name = ModParameters.EffectTexts.FirstOrDefault(x => x.Key.Equals("Happy_Om21341")).Value.Name;
                    desc = ModParameters.EffectTexts.FirstOrDefault(x => x.Key.Equals("Happy_Om21341")).Value.Desc;
                    _stack = 1;
                    break;
                case 2:
                    name = ModParameters.EffectTexts.FirstOrDefault(x => x.Key.Equals("Ecstatic_Om21341")).Value.Name;
                    desc = ModParameters.EffectTexts.FirstOrDefault(x => x.Key.Equals("Ecstatic_Om21341")).Value.Desc;
                    _stack = 2;
                    break;
                case 3:
                    name = ModParameters.EffectTexts.FirstOrDefault(x => x.Key.Equals("Manic_Om21341")).Value.Name;
                    desc = ModParameters.EffectTexts.FirstOrDefault(x => x.Key.Equals("Manic_Om21341")).Value.Desc;
                    _stack = 3;
                    break;
            }

            if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Happy_Om21341) is
                BattleUnitBuf_Happy_Om21341 buf)
            {
                buf.BufValue = stack;
            }
            else
            {
                buf = new BattleUnitBuf_Happy_Om21341
                {
                    BufValue = stack
                };
                owner.bufListDetail.AddBufWithoutDuplication(buf);
            }
        }

        public override void OnRoundStartAfter()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, _stack);
        }

        public void RemoveBuff()
        {
            EmotionalBurstUtil.DecreaseStacksBufType(owner, KeywordBuf.Quickness, _stack);
        }

        public void AfterInit()
        {
            OnRoundStartAfter();
        }

        public void InstantIncrease()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 1);
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            var isType = RndChance.Next(0, 100) <= _stack * 10;
            var value = _stack;
            if (behavior.GetDiceVanillaMax() - value < behavior.GetDiceVanillaMin() && isType)
                value = behavior.GetDiceVanillaMax() - behavior.GetDiceVanillaMin();
            var copyPassive = (PassiveAbility_Happy_Om21341)MemberwiseClone();
            copyPassive.isNegative = isType;
            UnitUtil.SetPassiveCombatLog(copyPassive, owner);
            behavior.ApplyDiceStatBonus(isType
                ? new DiceStatBonus { max = value * -1 }
                : new DiceStatBonus { max = value });
        }

        public override void OnRoundEnd()
        {
            ChangeAllCoinsToPositiveType();
        }

        private void ChangeAllCoinsToPositiveType()
        {
            owner.emotionDetail.totalEmotionCoins.RemoveAll(x => x.CoinType == EmotionCoinType.Negative);
            var allEmotionCoins = owner.emotionDetail.AllEmotionCoins.Where(x => x.CoinType == EmotionCoinType.Negative)
                .ToList();
            foreach (var coin in allEmotionCoins)
            {
                owner.emotionDetail.AllEmotionCoins.Remove(coin);
                owner.battleCardResultLog?.AddEmotionCoin(EmotionCoinType.Positive,
                    owner.emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive));
            }
        }
    }
}