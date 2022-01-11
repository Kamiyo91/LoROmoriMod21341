using EmotionalBurstPassive_Om21341.Passives;

namespace EmotionalBurstPassive_Om21341.Cards
{
    public class DiceCardSelfAbility_Stab_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
            if (card.target != null && card.target.passiveDetail.HasPassive<PassiveAbility_Happy_Om21341>())
            {
                owner.allyCardDetail.DrawCards(1);
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    dmgRate = 25
                });
            }

            if (owner.passiveDetail.HasPassive<PassiveAbility_Sad_Om21341>())
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    dmgRate = 25
                });
        }
    }
}