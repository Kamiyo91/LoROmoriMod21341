using EmotionalBurstPassive_Om21341.Cards;
using EmotionalBurstPassive_Om21341.Passives;

namespace Omori_Om21341.Cards
{
    public class DiceCardSelfAbility_BrokenHope_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
            if (!owner.passiveDetail.HasPassive<PassiveAbility_Sad_Om21341>()) return;
            DiceCardSelfAbility_Sad_Om21341.Activate(card.target);
        }
    }
}