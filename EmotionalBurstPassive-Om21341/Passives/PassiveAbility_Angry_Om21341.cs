using System.Linq;
using BigDLL4221.Models;
using BigDLL4221.Utils;
using OmoriMod_Om21341.BLL_Om21341;
using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Buffs;

namespace OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Passives
{
    public class PassiveAbility_Angry_Om21341 : PassiveAbilityBase
    {
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
                    name = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                        .FirstOrDefault(x => x.Key.Equals("Angry_Om21341")).Value.Name;
                    desc = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                        .FirstOrDefault(x => x.Key.Equals("Angry_Om21341")).Value.Desc;
                    _stack = 1;
                    break;
                case 2:
                    name = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                        .FirstOrDefault(x => x.Key.Equals("Enraged_Om21341")).Value.Name;
                    desc = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                        .FirstOrDefault(x => x.Key.Equals("Enraged_Om21341")).Value.Desc;
                    _stack = 2;
                    break;
                case 3:
                    name = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                        .FirstOrDefault(x => x.Key.Equals("Furious_Om21341")).Value.Name;
                    desc = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                        .FirstOrDefault(x => x.Key.Equals("Furious_Om21341")).Value.Desc;
                    _stack = 3;
                    break;
            }

            if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Angry_Om21341) is
                BattleUnitBuf_Angry_Om21341 buf)
            {
                buf.BufValue = stack;
            }
            else
            {
                buf = new BattleUnitBuf_Angry_Om21341
                {
                    BufValue = stack
                };
                owner.bufListDetail.AddBufWithoutDuplication(buf);
            }
        }

        public override void OnRoundStartAfter()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, _stack);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Disarm, _stack);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, _stack * 3);
        }

        public void RemoveBuff()
        {
            EmotionalBurstUtil.DecreaseStacksBufType(owner, KeywordBuf.Strength, _stack);
            EmotionalBurstUtil.DecreaseStacksBufType(owner, KeywordBuf.Disarm, _stack);
            EmotionalBurstUtil.DecreaseStacksBufType(owner, KeywordBuf.Vulnerable, _stack * 3);
        }

        public void InstantIncrease()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Disarm, 1);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, 3);
        }

        public void AfterInit()
        {
            OnRoundStartAfter();
        }

        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            UnitUtil.SetPassiveCombatLog(this, owner);
            owner.battleCardResultLog?.AddEmotionCoin(EmotionCoinType.Negative,
                owner.emotionDetail.CreateEmotionCoin(EmotionCoinType.Negative));
            return base.BeforeTakeDamage(attacker, dmg);
        }
    }
}