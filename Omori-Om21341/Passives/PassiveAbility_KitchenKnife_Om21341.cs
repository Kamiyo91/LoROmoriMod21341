namespace OmoriMod_Om21341.Omori_Om21341.Passives
{
    public class PassiveAbility_KitchenKnife_Om21341 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            behavior.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1, owner);
        }
    }
}