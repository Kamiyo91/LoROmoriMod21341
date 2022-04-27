using System.Linq;
using BLL_Om21341;
using EmotionalBurstPassive_Om21341.Buffs;

namespace Omori_Om21341.Cards
{
    public class DiceCardSelfAbility_OmoriMassAttackErase_Om21341 : DiceCardSelfAbilityBase
    {
        private bool _motionChanged;

        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return !owner.cardSlotDetail.cardAry.Exists(x =>
                x?.card?.GetID() == new LorId(OmoriModParameters.PackageId, 67));
        }

        public override void OnEndAreaAttack()
        {
            if (!_motionChanged) return;
            _motionChanged = false;
            owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
        }

        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            if (target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Afraid_Om21341) is
                BattleUnitBuf_Afraid_Om21341 buf)
            {
                buf.stack++;
            }
            else if (target.bufListDetail.GetReadyBufList().Find(x => x is BattleUnitBuf_Afraid_Om21341) is
                     BattleUnitBuf_Afraid_Om21341 buf2)
            {
                buf2.stack++;
            }
            else
            {
                buf = new BattleUnitBuf_Afraid_Om21341 { stack = 1 };
                target.bufListDetail.AddReadyBuf(buf);
            }
        }

        public override void OnApplyCard()
        {
            if (!string.IsNullOrEmpty(owner.UnitData.unitData.workshopSkin) ||
                owner.UnitData.unitData.bookItem != owner.UnitData.unitData.CustomBookItem) return;
            _motionChanged = true;
            owner.view.charAppearance.ChangeMotion(ActionDetail.Guard);
        }

        public override void OnReleaseCard()
        {
            _motionChanged = false;
            owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
        }

        public override void OnUseCard()
        {
            if (owner.emotionDetail.EmotionLevel <= 2) return;
            var dice = card.card.CreateDiceCardBehaviorList().FirstOrDefault();
            card.AddDice(dice);
            if (owner.emotionDetail.EmotionLevel > 4) card.AddDice(dice);
        }
    }
}