using Util_Om21341;

namespace Omori_Om21341.Passives
{
    public class PassiveAbility_MariProtection_Om21341 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            UnitUtil.ReadyCounterCard(owner, 78);
        }
    }
}