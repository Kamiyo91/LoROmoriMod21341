using EmotionalBurstPassive_Om21341.Passives;

namespace EmotionalBurstPassive_Om21341.Cards
{
    public class DiceCardSelfAbility_Headbutt_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            if (!owner.passiveDetail.HasPassive<PassiveAbility_Angry_Om21341>()) return;
            owner.allyCardDetail.DrawCards(1);
            owner.cardSlotDetail.RecoverPlayPoint(1);
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                dmg = 10,
                dmgRate = 25
            });
        }

        public override void OnSucceedAttack()
        {
            owner.TakeDamage(10);
        }
    }
}