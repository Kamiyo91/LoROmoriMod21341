namespace Util_Om21341.CommonBuffs
{
    public class BattleUnitBuf_ImmunityToStatusAliment_Om21341 : BattleUnitBuf
    {
        public override bool IsImmune(BufPositiveType posType)
        {
            return posType == BufPositiveType.Negative;
        }

        public override void OnRoundEnd()
        {
            Destroy();
        }
    }
}