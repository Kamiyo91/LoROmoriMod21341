using System.Linq;
using BigDLL4221.Utils;
using OmoriMod_Om21341.EmotionalBurstPassive_Om21341;

namespace OmoriMod_Om21341.Omori_Om21341.Cards
{
    public class DiceCardSelfAbility_Friends_Om21341 : DiceCardSelfAbilityBase
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
            if (UnitUtil.SupportCharCheck(owner) < 2 || !UnitUtil.ExcludeSupportChars(owner)
                    .All(EmotionalBurstUtil.CheckEmotionPassives)) return;
            foreach (var battleDiceCardModel in owner.allyCardDetail.GetAllDeck()
                         .FindAll(x => x != card.card && x.GetID() == card.card.GetID()))
            {
                battleDiceCardModel.GetBufList();
                battleDiceCardModel.AddCost(-1);
            }

            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, owner);
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 1, owner);
        }
    }
}