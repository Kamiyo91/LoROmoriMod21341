using BLL_Om21341.Models.MechUtilModels;

namespace BLL_Om21341.Extensions.MechUtilModelExtensions
{
    public class NpcMechUtil_OmoriModel : NpcMechUtilBaseModel
    {
        public int Phase { get; set; }
        public bool SingleUse { get; set; }
        public bool NotSuccumb { get; set; }
        public bool MapChanged { get; set; }
        public bool AttackMapChanged { get; set; }
        public bool NotSuccumbMech { get; set; }
    }
}