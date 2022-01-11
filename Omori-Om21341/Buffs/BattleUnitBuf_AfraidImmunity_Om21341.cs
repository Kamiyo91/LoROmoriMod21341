using EmotionalBurstPassive_Om21341.Buffs;

namespace Omori_Om21341.Buffs
{
    public class BattleUnitBuf_AfraidImmunity_Om21341 : BattleUnitBuf
    {
        public override bool IsImmune(BattleUnitBuf buf)
        {
            return buf is BattleUnitBuf_Afraid_Om21341 || base.IsImmune(buf);
        }
    }
}