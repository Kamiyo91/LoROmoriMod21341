namespace OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Passives
{
    public class PassiveAbility_Neutral_Om21341 : PassiveAbilityBase
    {
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            Hide();
        }

        public override void OnRoundStartAfter()
        {
            owner.allyCardDetail.DrawCards(1);
            owner.cardSlotDetail.RecoverPlayPoint(1);
        }
    }
}