using BLL_Om21341.Models.Enum;
using Util_Om21341;

namespace EmotionalBurstPassive_Om21341.Buffs
{
    public class BattleUnitBuf_Afraid_Om21341 : BattleUnitBuf
    {
        protected override string keywordId => "Afraid_Om21341";
        protected override string keywordIconId => "Afraid_Om21341";

        public override int GetDamageIncreaseRate()
        {
            return 50;
        }

        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            EmotionalBurstUtil.RemoveAllEmotionalPassives(owner, EmotionBufEnum.Afraid);
        }

        public override void OnRoundEnd()
        {
            stack--;
            if (stack < 1)
            {
                Destroy();
                return;
            }

            _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, stack, _owner);
        }

        public override bool IsCardChoosable(BattleDiceCardModel card)
        {
            return !UnitUtil.CantUseCardAfraid(card) && base.IsCardChoosable(card);
        }
    }
}