using BLL_Om21341;
using KamiyoStaticUtil.Utils;

namespace Omori_Om21341.Passives
{
    public class PassiveAbility_MariProtection_Om21341 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            UnitUtil.ReadyCounterCard(owner, 78, OmoriModParameters.PackageId);
        }
    }
}