using BLL_Om21341.Models.MechUtilModels;

namespace BLL_Om21341.Extensions.MechUtilModelExtensions
{
    public class MechUtil_OmoriModel : MechUtilBaseModel
    {
        public bool EgoMapAttackused;
        public bool MapChanged { get; set; }
        public bool NotSuccumb { get; set; }
        public int RechargeCount { get; set; }
    }
}