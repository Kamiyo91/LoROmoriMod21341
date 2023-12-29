using System.Collections.Generic;
using System.Linq;
using LOR_XML;
using OmoriMod_Om21341.BLL_Om21341;
using OmoriMod_Om21341.BLL_Om21341.Enum;
using OmoriMod_Om21341.EmotionalBurstPassive_Om21341;
using OmoriMod_Om21341.Omori_Om21341.Buffs;
using UnityEngine;
using UtilLoader21341;
using UtilLoader21341.Util;

namespace OmoriMod_Om21341.Omori_Om21341.Passives
{
    public class PassiveAbility_Omori_Om21341 : PassiveAbilityBase
    {
        private readonly List<AbnormalityCardDialog> _egoDialog = new List<AbnormalityCardDialog>
        {
            new AbnormalityCardDialog
            {
                id = "Omori",
                dialog = ModParameters.LocalizedItems[OmoriModParameters.PackageId].EffectTexts
                    .FirstOrDefault(x => x.Key.Equals("OmoriSurvive1_Om21341"))
                    .Value.Desc
            }
        };

        private bool _attackMapUsed;
        private bool _egoActive;

        private bool _mapChanged;
        private int _recharge;
        private bool _survived;

        public override void OnWaveStart()
        {
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_AfraidImmunity_Om21341());
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
            if (_recharge != 0) return base.BeforeTakeDamage(attacker, dmg);
            if (owner.SurviveCheck<BattleUnitBuf>(dmg, 1, ref _survived, 75)) _recharge = 5;
            return base.BeforeTakeDamage(attacker, dmg);
        }

        public override void OnRoundStartAfter()
        {
            if (_egoActive || !_survived) return;
            if (!OmoriUtil.EgoActive(owner, ref _egoActive, _egoDialog)) return;
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_UntargetableOmori_Om21341());
            OmoriUtil.ChangeMapToSuccumbState(ref _mapChanged);
        }

        public override void OnRoundEnd()
        {
            _recharge--;
            _recharge = Mathf.Clamp(_recharge, 0, 5);
            if (_recharge == 0)
            {
                _survived = false;
                _egoActive = false;
            }

            OmoriUtil.ReturnFromEgoMap(ref _mapChanged);
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            OmoriUtil.ChangeToEgoMap(curCard.card.GetID(), ref _attackMapUsed);
        }

        public override void OnRoundEndTheLast_ignoreDead()
        {
            OmoriUtil.ReturnFromEgoMap(ref _mapChanged);
            OmoriUtil.ReturnFromEgoAttackMap(ref _attackMapUsed);
        }
    }
}