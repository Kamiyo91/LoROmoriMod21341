using System.Linq;
using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Passives;
using UtilLoader21341.Util;

namespace OmoriMod_Om21341.Omori_Om21341.Cards
{
    public class DiceCardSelfAbility_Picnic_Om21341 : DiceCardSelfAbilityBase
    {
        public override string[] Keywords
        {
            get
            {
                return new[]
                {
                    "FriendsPage_Om21341"
                };
            }
        }

        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
            if (UnitUtil.SupportCharCheck(owner) < 2 || !UnitUtil.ExcludeSupportChars(owner).All(x =>
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