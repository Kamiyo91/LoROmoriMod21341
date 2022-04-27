using BLL_Om21341;
using BLL_Om21341.Enum;
using EmotionalBurstPassive_Om21341.Passives;

namespace EmotionalBurstPassive_Om21341.Cards
{
    public class DiceCardSelfAbility_Sad_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            Activate(targetUnit);
            self.exhaust = true;
            EmotionalBurstUtil.RemoveEmotionalBurstCards(unit);
        }

        public static void Activate(BattleUnitModel unit)
        {
            EmotionalBurstUtil.RemoveAllEmotionalPassives(unit, EmotionBufEnum.Sad);
            if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_Sad_Om21341) is
                PassiveAbility_Sad_Om21341 passiveSad)
            {
                var stacks = passiveSad.GetStack();
                if (stacks >= 3) return;
                passiveSad.ChangeNameAndSetStacks(stacks + 1);
                passiveSad.InstantIncrease();
                return;
            }

            var passive =
                unit.passiveDetail.AddPassive(new LorId(OmoriModParameters.PackageId, 31)) as
                    PassiveAbility_Sad_Om21341;
            passive?.ChangeNameAndSetStacks(1);
            passive?.AfterInit();
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