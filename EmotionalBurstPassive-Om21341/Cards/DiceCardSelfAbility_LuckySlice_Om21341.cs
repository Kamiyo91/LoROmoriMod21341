using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Passives;

namespace OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Cards
{
    public class DiceCardSelfAbility_LuckySlice_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            if (!owner.passiveDetail.HasPassive<PassiveAbility_Happy_Om21341>()) return;
            owner.cardSlotDetail.RecoverPlayPointByCard(2);
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                dmgRate = 10
            });
        }
    }
}