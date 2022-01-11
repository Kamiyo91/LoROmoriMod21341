using EmotionalBurstPassive_Om21341.Buffs;
using EmotionalBurstPassive_Om21341.Passives;

namespace EmotionalBurstPassive_Om21341.Cards
{
    public class DiceCardSelfAbility_Mock_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            owner.allyCardDetail.DrawCards(1);
            if (card.target == null || !card.target.passiveDetail.HasPassive<PassiveAbility_Angry_Om21341>()) return;
            if (card.target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Mock_Om21341) is
                BattleUnitBuf_Mock_Om21341 buf)
            {
                if (buf.stack < 3) buf.stack++;
            }
            else
            {
                buf = new BattleUnitBuf_Mock_Om21341
                {
                    stack = 1
                };
                card.target.bufListDetail.AddBufWithoutDuplication(buf);
            }
        }
    }
}