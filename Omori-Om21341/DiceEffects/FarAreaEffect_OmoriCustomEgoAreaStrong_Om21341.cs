using OmoriMod_Om21341.Util_Om21341.CommonMaps;
using Sound;
using UnityEngine;

namespace OmoriMod_Om21341.Omori_Om21341.DiceEffects
{
    public class FarAreaEffect_OmoriCustomEgoAreaStrong_Om21341 : FarAreaEffect
    {
        private bool _damaged;
        private float _elapsed;
        private bool _ended;
        private OmoriBoomEffectMap_Om21341MapManager _map;

        private OmoriBoomEffectMap_Om21341MapManager Map
        {
            get
            {
                if (_map == null)
                    _map =
                        SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject as
                            OmoriBoomEffectMap_Om21341MapManager;
                return _map;
            }
        }

        public override void OnGiveDamage()
        {
            base.OnGiveDamage();
            var map = Map;
            if (map != null) OmoriBoomEffectMap_Om21341MapManager.BoomFirst();
            PrintSound();
            isRunning = false;
        }

        public override void Init(BattleUnitModel self, params object[] args)
        {
            base.Init(self, args);
            SoundEffectPlayer.PlaySound("Battle/Roland_Phase4_CryStart");
        }

        protected virtual void PrintSound()
        {
            SoundEffectPlayer.PlaySound("Battle/Roland_Phase2_Windblast");
        }

        public override void OnEffectEnd()
        {
            base.OnEffectEnd();
            _isDoneEffect = true;
            gameObject.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();
            _elapsed += Time.deltaTime;
            if (!_damaged && _elapsed >= 0.4f)
            {
                _damaged = true;
                OnGiveDamage();
            }

            if (!_ended && _elapsed >= 0.8f)
            {
                _ended = true;
                OnEffectEnd();
            }
        }
    }
}