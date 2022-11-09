using System;
using UnityEngine;

namespace OmoriMod_Om21341.Omori_Om21341.DiceEffects
{
    public class BehaviourAction_OmoriHackAway_Om21341 : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            _self = self;
            var omoriHackAway = new GameObject().AddComponent<FarAreaEffect_OmoriHackAway_Om21341>();
            omoriHackAway.Init(self, Array.Empty<object>());
            return omoriHackAway;
        }
    }
}