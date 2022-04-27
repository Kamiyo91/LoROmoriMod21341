using BLL_Om21341;
using EmotionalBurstPassive_Om21341.Buffs;
using EmotionalBurstPassive_Om21341.Passives;

namespace EmotionalBurstPassive_Om21341.Cards
{
    public class DiceCardSelfAbility_Neutral_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            Activate(targetUnit);
            self.exhaust = true;
            EmotionalBurstUtil.RemoveEmotionalBurstCards(unit);
        }

        public static void Activate(BattleUnitModel unit)
        {
            EmotionalBurstUtil.RemoveAllEmotionalPassives(unit);
            AddNeutralPassive(unit);
        }

        private static void AddNeutralPassive(BattleUnitModel unit)
        {
            if (unit.passiveDetail.HasPassive<PassiveAbility_Neutral_Om21341>()) return;
            unit.passiveDetail.AddPassive(new LorId(OmoriModParameters.PackageId, 32));
            unit.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_Neutral_Om21341());
            if (unit.faction == Faction.Player) unit.passiveDetail.OnCreated();
        }

        public override bool IsTargetableAllUnit()
        {
            return true;
        }

        public override bool IsTargetableSelf()
        {
            return true;
        }
    }
}