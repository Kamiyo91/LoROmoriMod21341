namespace EmotionalBurstPassive_Om21341.Buffs
{
    public class BattleUnitBuf_Mock_Om21341 : BattleUnitBuf
    {
        protected override string keywordId => "Mock_Om21341";
        protected override string keywordIconId => "Mock_Om21341";

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(
                new DiceStatBonus
                {
                    min = -stack,
                    max = -stack
                });
        }

        public override void OnRoundEnd()
        {
            Destroy();
        }
    }
}