using EmotionalBurstPassive_Om21341.Buffs;

namespace Omori_Om21341.Cards
{
    public class DiceCardSelfAbility_Hopeless_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
            if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Sad_Om21341) is
                    BattleUnitBuf_Sad_Om21341 buf && buf.BufValue > 2)
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    power = 2
                });
        }
    }
}