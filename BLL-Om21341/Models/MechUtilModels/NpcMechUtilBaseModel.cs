using System;

namespace BLL_Om21341.Models.MechUtilModels
{
    public class NpcMechUtilBaseModel : MechUtilBaseModel
    {
        public int MechHp { get; set; }
        public bool HasMechOnHp { get; set; }
        public int Counter { get; set; }
        public int MaxCounter { get; set; }
        public int SpecialCardCost { get; set; }
        public Type SpecialBufType { get; set; }
        public bool ReloadMassAttackOnLethal { get; set; }
        public bool OneTurnCard { get; set; }
        public LorId LorIdEgoMassAttack { get; set; }
        public bool MassAttackStartCount { get; set; }
    }
}