using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Passives;

namespace OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Cards
{
    public class DiceCardSelfAbility_Comeback_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            owner.allyCardDetail.DrawCards(1);
            if (owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_Sad_Om21341) is PassiveAbility_Sad_Om21341
                passive)
            {
                owner.RecoverHP(10 * passive.GetStack());
                owner.breakDetail.RecoverBreak(10 * passive.GetStack());
                owner.cardSlotDetail.RecoverPlayPoint(passive.GetStack());
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, passive.GetStack(), owner);
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, passive.GetStack(), owner);
            }

            DiceCardSelfAbility_Happy_Om21341.Activate(owner);
        }

        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.hp < owner.MaxHp * 0.25f;
        }
    }
}