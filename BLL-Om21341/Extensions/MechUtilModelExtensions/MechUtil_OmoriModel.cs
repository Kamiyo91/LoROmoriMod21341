using BigDLL4221.Models;

namespace OmoriMod_Om21341.BLL_Om21341.Extensions.MechUtilModelExtensions
{
    public class MechUtil_OmoriModel : MechUtilBaseModel
    {
        public bool EgoMapAttackUsed;
        public bool MapChanged { get; set; }
        public bool NotSuccumb { get; set; }
        public int RechargeCount { get; set; }
    }
}