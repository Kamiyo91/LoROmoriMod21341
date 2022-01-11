using BLL_Om21341.Models.Enum;

namespace EmotionalBurstPassive_Om21341.Passives
{
    public class PassiveAbility_EmotionalBurst_Om21341 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            PrepareEmotionalBurstCards();
        }

        private void PrepareEmotionalBurstCards()
        {
            EmotionBufEnum emotionCardType;
            switch (id.id)
            {
                case 28:
                    emotionCardType = EmotionBufEnum.Angry;
                    break;
                case 46:
                    emotionCardType = EmotionBufEnum.Happy;
                    break;
                case 47:
                    emotionCardType = EmotionBufEnum.Sad;
                    break;
                case 48:
                    emotionCardType = EmotionBufEnum.Neutral;
                    break;
                default:
                    return;
            }

            EmotionalBurstUtil.AddEmotionalBurstCard(owner, emotionCardType);
        }

        public override void OnRoundEnd()
        {
            EmotionalBurstUtil.RemoveEmotionalBurstCards(owner);
        }
    }
}