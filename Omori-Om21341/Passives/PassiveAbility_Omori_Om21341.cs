using System.Collections.Generic;
using System.Linq;
using BigDLL4221.Models;
using BigDLL4221.Utils;
using LOR_XML;
using OmoriMod_Om21341.BLL_Om21341;
using OmoriMod_Om21341.BLL_Om21341.Enum;
using OmoriMod_Om21341.BLL_Om21341.Extensions.MechUtilModelExtensions;
using OmoriMod_Om21341.EmotionalBurstPassive_Om21341;
using OmoriMod_Om21341.Omori_Om21341.Buffs;
using OmoriMod_Om21341.Omori_Om21341.MechUtil;

namespace OmoriMod_Om21341.Omori_Om21341.Passives
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
                SurviveHp = 1,
                RecoverToHp = 20,
                RechargeCount = 5,
                RecoverLightOnSurvive = true,
                Survive = true,
                EgoOptions = new Dictionary<int, EgoOptions>
                {
                    {
                        0, new EgoOptions(new BattleUnitBuf_UntargetableOmori_Om21341(),
                            egoAbDialogList: new List<AbnormalityCardDialog>
                            {
                                new AbnormalityCardDialog
                                {
                                    id = "Omori",
                                    dialog = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                                        .FirstOrDefault(x => x.Key.Equals("OmoriSurvive1_Om21341"))
                                        .Value.Desc
                                }
                            })
                    }
                },
                EgoMaps = new Dictionary<LorId, MapModel> { { new LorId(OmoriModParameters.PackageId, 66), null } }
            });
            owner.personalEgoDetail.AddCard(new LorId(OmoriModParameters.PackageId, 66));
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
            _util.RechargeCheck();
            _util.IncreaseRecharge();
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            _util.ChangeToEgoMap(curCard.card.GetID());
        }

        public override void OnRoundEndTheLast_ignoreDead()
        {
            _util.ReturnFromEgoMap();
            _util.ReturnFromEgoAttackMap();
        }
    }
}