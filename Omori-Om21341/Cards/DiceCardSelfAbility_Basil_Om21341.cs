using EmotionalBurstPassive_Om21341.Passives;

namespace Omori_Om21341.Cards
{
    public class DiceCardSelfAbility_Basil_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.cardSlotDetail.RecoverPlayPoint(1);
            if (!owner.passiveDetail.HasPassive<PassiveAbility_Happy_Om21341>()) return;
            foreach (var battleDiceCardModel in owner.allyCardDetail.GetAllDeck()
                         .FindAll(x => x != card.card && x.GetID() == card.card.GetID()))
            {
                battleDiceCardModel.GetBufList();
                battleDiceCardModel.AddCost(-1);
            }

            owner.cardSlotDetail.RecoverPlayPoint(1);
        }
    }
}