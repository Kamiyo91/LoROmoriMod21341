using System;
using UnityEngine;

namespace Omori_Om21341.DiceEffects
{
    public class BehaviourAction_OmoriCustomEgoAreaStrong_Om21341 : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            _self = self;
            var farAreaeffectBlackSilence4ThAreaStrong =
                new GameObject().AddComponent<FarAreaEffect_OmoriCustomEgoAreaStrong_Om21341>();
            farAreaeffectBlackSilence4ThAreaStrong.Init(self, Array.Empty<object>());
            return farAreaeffectBlackSilence4ThAreaStrong;
        }
    }
}