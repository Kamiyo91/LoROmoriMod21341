using OmoriMod_Om21341.EmotionalBurstPassive_Om21341.Cards;

namespace OmoriMod_Om21341.Omori_Om21341.Cards
{
    public class DiceCardSelfAbility_SomethingGuilt_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            DiceCardSelfAbility_Sad_Om21341.Activate(owner);
        }
    }
}