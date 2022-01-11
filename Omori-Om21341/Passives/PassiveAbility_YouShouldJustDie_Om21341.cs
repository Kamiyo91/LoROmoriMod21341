namespace Omori_Om21341.Passives
{
    public class PassiveAbility_YouShouldJustDie_Om21341 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(Faction.Player)).bufListDetail
                .AddKeywordBufByEtc(KeywordBuf.NullifyPower, 1, owner);
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 3, owner);
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 3, owner);
        }
    }
}