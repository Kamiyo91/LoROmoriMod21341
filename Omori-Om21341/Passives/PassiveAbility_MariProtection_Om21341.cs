using OmoriMod_Om21341.BLL_Om21341;
using UtilLoader21341.Util;

namespace OmoriMod_Om21341.Omori_Om21341.Passives
{
    public class PassiveAbility_MariProtection_Om21341 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            owner.ReadyCounterCard(78, OmoriModParameters.PackageId);
        }
    }
}