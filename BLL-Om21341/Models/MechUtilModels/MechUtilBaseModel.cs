using System;
using System.Collections.Generic;
using BLL_Om21341.Models.Enum;
using LOR_XML;

namespace BLL_Om21341.Models.MechUtilModels
{
    public class MechUtilBaseModel
    {
        public BattleUnitModel Owner { get; set; }
        public int Hp { get; set; }
        public int SetHp { get; set; }
        public bool Survive { get; set; }
        public bool HasEgo { get; set; }
        public bool HasEgoAttack { get; set; }
        public bool EgoActivated { get; set; }
        public bool RefreshUI { get; set; }
        public bool IsSummonEgo { get; set; }
        public bool RecoverLightOnSurvive { get; set; }
        public bool EgoAttackCardExpire { get; set; }
        public bool HasAdditionalPassive { get; set; }
        public string SkinName { get; set; }
        public bool DieOnFightEnd { get; set; }
        public List<AbnormalityCardDialog> SurviveAbDialogList { get; set; }
        public List<AbnormalityCardDialog> EgoAbDialogList { get; set; }
        public bool HasEgoAbDialog { get; set; }
        public bool HasSurviveAbDialog { get; set; }
        public bool NearDeathBuffExist { get; set; }
        public AbColorType SurviveAbDialogColor { get; set; }
        public AbColorType EgoAbColorColor { get; set; }
        public Type NearDeathBuffType { get; set; }
        public Type EgoType { get; set; }
        public LorId[] LorIdArray { get; set; } = null;
        public LorId EgoCardId { get; set; }
        public LorId SecondaryEgoCardId { get; set; }
        public LorId EgoAttackCardId { get; set; }
        public LorId AdditionalPassiveId { get; set; }
    }
}