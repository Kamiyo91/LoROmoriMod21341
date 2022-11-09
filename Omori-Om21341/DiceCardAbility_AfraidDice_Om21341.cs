using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Buffs;

namespace OmoriMod_Om21341.Omori_Om21341
{
    public class DiceCardAbility_AfraidDice_Om21341 : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Afraid_Om21341) is
                BattleUnitBuf_Afraid_Om21341 buf)
            {
                buf.stack++;
            }
            else if (target.bufListDetail.GetReadyBufList().Find(x => x is BattleUnitBuf_Afraid_Om21341) is
                     BattleUnitBuf_Afraid_Om21341 buf2)
            {
                buf2.stack++;
            }
            else
            {
                buf = new BattleUnitBuf_Afraid_Om21341 { stack = 1 };
                target.bufListDetail.AddReadyBuf(buf);
            }
        }
    }
}