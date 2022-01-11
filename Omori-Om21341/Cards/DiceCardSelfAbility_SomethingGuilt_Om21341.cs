using EmotionalBurstPassive_Om21341.Cards;

namespace Omori_Om21341.Cards
{
    public class DiceCardSelfAbility_SomethingGuilt_Om21341 : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            DiceCardSelfAbility_Sad_Om21341.Activate(owner);
        }
    }
}