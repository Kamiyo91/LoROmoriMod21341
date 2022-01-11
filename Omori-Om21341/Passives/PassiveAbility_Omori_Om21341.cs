using System.Collections.Generic;
using System.Linq;
using BLL_Om21341.Extensions.MechUtilModelExtensions;
using BLL_Om21341.Models;
using BLL_Om21341.Models.Enum;
using EmotionalBurstPassive_Om21341;
using LOR_XML;
using Omori_Om21341.Buffs;
using Omori_Om21341.MechUtil;
using Util_Om21341;

namespace Omori_Om21341.Passives
{
    public class PassiveAbility_Omori_Om21341 : PassiveAbilityBase
    {
        private MechUtil_Omori _util;

        public override void OnWaveStart()
        {
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_AfraidImmunity_Om21341());
            _util = new MechUtil_Omori(new MechUtil_OmoriModel
            {
                Owner = owner,
                Hp = 0,
                SetHp = 20,
                RechargeCount = 5,
                RecoverLightOnSurvive = true,
                HasEgoAbDialog = true,
                Survive = true,
                HasEgo = true,
                EgoType = typeof(BattleUnitBuf_UntargetableOmori_Om21341),
                EgoAbDialogList = new List<AbnormalityCardDialog>
                {
                    new AbnormalityCardDialog
                    {
                        id = "Omori",
                        dialog = ModParameters.EffectTexts.FirstOrDefault(x => x.Key.Equals("OmoriSurvive1_Om21341"))
                            .Value.Desc
                    }
                },
                EgoAbColorColor = AbColorType.Negative
            });
            owner.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 66));
            UnitUtil.CheckSkinProjection(owner);
        }

        public override void OnRoundStart()
        {
            EmotionalBurstUtil.RemoveEmotionalBurstCards(owner);
            EmotionalBurstUtil.AddEmotionalBurstCard(owner, EmotionBufEnum.All);
        }

        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            _util.SurviveCheck(dmg);
            return base.BeforeTakeDamage(attacker, dmg);
        }

        public override void OnRoundStartAfter()
        {
            if (!_util.GetSuccumbStatus()) return;
            _util.SetSuccumbStatus(false);
            _util.EgoActive();
            _util.ChangeMap(owner);
        }

        public override void OnRoundEnd()
        {
            _util.ReturnFromEgoMap();
            _util.RechargeCheck();
            _util.IncreaseRecharge();
        }
    }
}