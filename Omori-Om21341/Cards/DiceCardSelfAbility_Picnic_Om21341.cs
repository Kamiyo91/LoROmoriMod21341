using System.Linq;
using EmotionalBurstPassive_Om21341.Passives;

namespace Omori_Om21341.Cards
{
    public class DiceCardSelfAbility_Picnic_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
            if (BattleObjectManager.instance.GetAliveList(Faction.Player).Count < 2 || !BattleObjectManager.instance
                    .GetAliveList(Faction.Player).All(x =>
                        x.passiveDetail.PassiveList.Exists(y => !y.destroyed && y is PassiveAbility_Happy_Om21341)))
                return;
            foreach (var battleDiceCardModel in owner.allyCardDetail.GetAllDeck()
                         .FindAll(x => x != card.card && x.GetID() == card.card.GetID()))
            {
                battleDiceCardModel.GetBufList();
                battleDiceCardModel.AddCost(-1);
            }
            foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player).Where(x => x != owner))
            {
                unit.allyCardDetail.DrawCards(1);
                unit.cardSlotDetail.RecoverPlayPoint(1);
            }
        }
    }
}