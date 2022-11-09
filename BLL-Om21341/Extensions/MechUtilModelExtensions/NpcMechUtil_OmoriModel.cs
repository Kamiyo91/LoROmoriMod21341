using System.Collections.Generic;
using BigDLL4221.Enum;
using BigDLL4221.Models;
using LOR_XML;

namespace OmoriMod_Om21341.BLL_Om21341.Extensions.MechUtilModelExtensions
{
    public class NpcMechUtil_OmoriModel : NpcMechUtilBaseModel
    {
        public NpcMechUtil_OmoriModel(string saveDataId, Dictionary<int, EgoOptions> egoOptions = null,
            int additionalStartDraw = 0, int surviveHp = 0, int recoverToHp = 0, string originalSkinName = "",
            bool survive = false, bool recoverLightOnSurvive = false, bool dieOnFightEnd = false,
            DamageOptions damageOptions = null, List<AbnormalityCardDialog> surviveAbDialogList = null,
            AbColorType surviveAbDialogColor = AbColorType.Negative, BattleUnitBuf nearDeathBuffType = null,
            List<PermanentBuffOptions> permanentBuffList = null,
            Dictionary<LorId, PersonalCardOptions> personalCards = null, bool reusableEgo = false,
            bool reviveOnDeath = false, int recoverHpOnRevive = 0,
            List<AbnormalityCardDialog> reviveAbDialogList = null,
            AbColorType reviveAbDialogColor = AbColorType.Negative,
            Dictionary<int, MechPhaseOptions> mechOptions = null, bool reloadMassAttackOnLethal = true,
            bool massAttackStartCount = false, SpecialCardOption specialCardOptions = null,
            LorId firstEgoFormCard = null, string egoSaveDataId = "", Dictionary<LorId, MapModel> egoMaps = null,
            List<BattleUnitBuf> addBuffsOnPlayerUnitsAtStart = null, bool phaseChangingRoundStartAfter = false,
            bool forcedRetreatOnRevive = false, bool originalSkinIsBaseGame = false) : base(saveDataId, egoOptions,
            additionalStartDraw, surviveHp, recoverToHp, originalSkinName, survive, recoverLightOnSurvive,
            dieOnFightEnd, damageOptions, surviveAbDialogList, surviveAbDialogColor, nearDeathBuffType,
            permanentBuffList, personalCards, reusableEgo, reviveOnDeath, recoverHpOnRevive, reviveAbDialogList,
            reviveAbDialogColor, mechOptions, reloadMassAttackOnLethal, massAttackStartCount, specialCardOptions,
            firstEgoFormCard, egoSaveDataId, egoMaps, addBuffsOnPlayerUnitsAtStart, phaseChangingRoundStartAfter,
            forcedRetreatOnRevive, originalSkinIsBaseGame)
        {
        }

        public new int Phase { get; set; }
        public bool SingleUse { get; set; }
        public bool NotSuccumb { get; set; }
        public bool MapChanged { get; set; }
        public bool AttackMapChanged { get; set; }
        public bool NotSuccumbMech { get; set; }
    }
}