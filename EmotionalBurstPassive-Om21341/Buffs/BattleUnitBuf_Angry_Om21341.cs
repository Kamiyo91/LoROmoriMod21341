using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Passives;

namespace OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Buffs
{
    public class BattleUnitBuf_Angry_Om21341 : BattleUnitBuf
    {
        private BattleUnitModel _attacker;

        public BattleUnitBuf_Angry_Om21341()
        {
            stack = 0;
        }

        public int BufValue { get; set; }
        public override int paramInBufDesc => 0;

        protected override string keywordId =>
            BufValue == 1 ? "Angry_Om21341" : BufValue == 2 ? "Enraged_Om21341" : "Furious_Om21341";

        protected override string keywordIconId => "Aubrey_Om21341";

        public override void BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            _attacker = attacker;
            base.BeforeTakeDamage(attacker, dmg);
        }

        public override int GetDamageReductionRate()
        {
            if (_attacker != null && _attacker.passiveDetail.HasPassive<PassiveAbility_Sad_Om21341>())
                return 10 * BufValue;
            return base.GetDamageReductionRate();
        }

        public override int GetBreakDamageReductionRate()
        {
            if (_attacker != null && _attacker.passiveDetail.HasPassive<PassiveAbility_Sad_Om21341>())
                return 10 * BufValue;
            return base.GetBreakDamageReductionRate();
        }

        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (!behavior.card.target.passiveDetail.HasPassive<PassiveAbility_Sad_Om21341>()) return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                dmgRate = 10 * BufValue,
                breakRate = 10 * BufValue
            });
        }
    }
}