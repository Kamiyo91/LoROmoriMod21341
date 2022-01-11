using BLL_Om21341.Models;
using BLL_Om21341.Models.Enum;
using EmotionalBurstPassive_Om21341.Passives;

namespace EmotionalBurstPassive_Om21341.Cards
{
    public class DiceCardSelfAbility_Angry_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            Activate(targetUnit);
            self.exhaust = true;
            EmotionalBurstUtil.RemoveEmotionalBurstCards(unit);
        }

        public static void Activate(BattleUnitModel unit)
        {
            EmotionalBurstUtil.RemoveAllEmotionalPassives(unit, EmotionBufEnum.Angry);
            if (unit.passiveDetail.PassiveList.Find(x =>
                    x is PassiveAbility_Angry_Om21341) is PassiveAbility_Angry_Om21341 passiveAngry)
            {
                var stacks = passiveAngry.GetStack();
                if (stacks >= 3) return;
                passiveAngry.ChangeNameAndSetStacks(stacks + 1);
                passiveAngry.InstantIncrease();
                return;
            }

            var passive =
                unit.passiveDetail.AddPassive(new LorId(ModParameters.PackageId, 30)) as
                    PassiveAbility_Angry_Om21341;
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