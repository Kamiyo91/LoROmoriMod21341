using BigDLL4221.Utils;
using OmoriMod_Om21341.BLL_Om21341;

namespace OmoriMod_Om21341.Omori_Om21341.Passives
{
    public class PassiveAbility_MariProtection_Om21341 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            UnitUtil.ReadyCounterCard(owner, 78, OmoriModParameters.PackageId);
        }
    }
}