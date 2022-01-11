using System.Linq;
using BLL_Om21341.Models;
using BLL_Om21341.Models.Enum;
using EmotionalBurstPassive_Om21341.Buffs;
using EmotionalBurstPassive_Om21341.Passives;

namespace EmotionalBurstPassive_Om21341
{
    public static class EmotionalBurstUtil
    {
        public static void RemoveEmotionalBurstCards(BattleUnitModel unit)
        {
            unit.personalEgoDetail.RemoveCard(new LorId(ModParameters.PackageId, 32));
            unit.personalEgoDetail.RemoveCard(new LorId(ModParameters.PackageId, 33));
            unit.personalEgoDetail.RemoveCard(new LorId(ModParameters.PackageId, 34));
            unit.personalEgoDetail.RemoveCard(new LorId(ModParameters.PackageId, 35));
        }

        public static void AddEmotionalBurstCard(BattleUnitModel unit, EmotionBufEnum type)
        {
            switch (type)
            {
                case EmotionBufEnum.Neutral:
                    unit.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 35));
                    return;
                case EmotionBufEnum.Angry:
                    unit.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 32));
                    return;
                case EmotionBufEnum.Happy:
                    unit.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 34));
                    return;
                case EmotionBufEnum.Sad:
                    unit.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 33));
                    return;
                case EmotionBufEnum.All:
                case EmotionBufEnum.Afraid:
                default:
                    unit.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 32));
                    unit.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 33));
                    unit.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 34));
                    unit.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 35));
                    return;
            }
        }

        public static bool CheckEmotionPassives(BattleUnitModel unit)
        {
            return unit.passiveDetail.PassiveList.Exists(x => !x.destroyed && x is PassiveAbility_Neutral_Om21341) ||
                   unit.passiveDetail.PassiveList.Exists(x => !x.destroyed && x is PassiveAbility_Happy_Om21341) ||
                   unit.passiveDetail.PassiveList.Exists(x => !x.destroyed && x is PassiveAbility_Angry_Om21341) ||
                   unit.passiveDetail.PassiveList.Exists(x => !x.destroyed && x is PassiveAbility_Sad_Om21341);
        }

        public static void RemoveAllEmotionalPassives(BattleUnitModel unit,
            EmotionBufEnum type = EmotionBufEnum.Neutral)
        {
            if (type != EmotionBufEnum.Afraid)
                if (unit.bufListDetail.GetActivatedBufList().Exists(x => x is BattleUnitBuf_Afraid_Om21341))
                    unit.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_Afraid_Om21341));
            if (type != EmotionBufEnum.Neutral)
            {
                if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_Neutral_Om21341) is
                    PassiveAbility_Neutral_Om21341 passiveAbilityNeutral)
                    unit.passiveDetail.DestroyPassive(passiveAbilityNeutral);
                if (unit.bufListDetail.GetActivatedBufList().Exists(x => x is BattleUnitBuf_Neutral_Om21341))
                    unit.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_Neutral_Om21341));
            }

            if (type != EmotionBufEnum.Happy)
            {
                if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_Happy_Om21341) is
                    PassiveAbility_Happy_Om21341 passiveAbilityHappy)
                {
                    passiveAbilityHappy.RemoveBuff();
                    unit.passiveDetail.DestroyPassive(passiveAbilityHappy);
                }

                if (unit.bufListDetail.GetActivatedBufList().Exists(x => x is BattleUnitBuf_Happy_Om21341))
                    unit.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_Happy_Om21341));
            }

            if (type != EmotionBufEnum.Angry)
            {
                if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_Angry_Om21341) is
                    PassiveAbility_Angry_Om21341 passiveAbilityAngry)
                {
                    passiveAbilityAngry.RemoveBuff();
                    unit.passiveDetail.DestroyPassive(passiveAbilityAngry);
                }

                if (unit.bufListDetail.GetActivatedBufList().Exists(x => x is BattleUnitBuf_Angry_Om21341))
                    unit.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_Angry_Om21341));
            }

            if (type == EmotionBufEnum.Sad) return;
            if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_Sad_Om21341) is
                PassiveAbility_Sad_Om21341 passiveAbilitySad)
            {
                passiveAbilitySad.RemoveBuff();
                unit.passiveDetail.DestroyPassive(passiveAbilitySad);
            }

            if (unit.bufListDetail.GetActivatedBufList().Exists(x => x is BattleUnitBuf_Sad_Om21341))
                unit.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_Sad_Om21341));
        }

        public static void DecreaseStacksBufType(BattleUnitModel owner, KeywordBuf bufType, int stacks)
        {
            var buf = owner.bufListDetail.GetActivatedBufList().FirstOrDefault(x => x.bufType == bufType);
            if (buf != null) buf.stack -= stacks;
            if (buf != null && buf.stack < 1) owner.bufListDetail.RemoveBuf(buf);
        }
    }
}