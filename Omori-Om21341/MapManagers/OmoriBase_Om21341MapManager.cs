using System.Linq;
using CustomMapUtility;
using OmoriMod_Om21341.BLL_Om21341;
using OmoriMod_Om21341.Omori_Om21341.Passives;
using OmoriMod_Om21341.Util_Om21341.CommonMaps;
using UnityEngine;
using UtilLoader21341.Util;

namespace OmoriMod_Om21341.Omori_Om21341.MapManagers
{
#pragma warning disable
    public class OmoriBase_Om21341MapManager : OmoriBoomEffectMap_Om21341MapManager
    {
        private readonly CustomMapHandler _cmh = CustomMapHandler.GetCMU(OmoriModParameters.PackageId);
        private AudioClip _introClip;
        private AudioClip _loopClip;
        private AudioSource _overlay;
        private PassiveAbility_OmoriNpc_Om21341 passive;

        public override void EnableMap(bool b)
        {
            if (b && !isEgo) mapBgm = new[] { _cmh.StartEnemyTheme_LoopPair(_introClip, _loopClip) };

            base.EnableMap(b);
        }

        public override void InitializeMap()
        {
            base.InitializeMap();
            _introClip = _cmh.GetAudioClip("boss_OMORI.ogg");
            var unit = BattleObjectManager.instance.GetAliveList().FirstOrDefault(x =>
                x.passiveDetail.PassiveList.Any(y => y is PassiveAbility_OmoriNpc_Om21341));
            if (unit == null) return;
            passive = unit.GetActivePassive<PassiveAbility_OmoriNpc_Om21341>();
            _loopClip = passive?.LoopClip ??
                        _cmh.ClipCut(_introClip, 1860207, 9305332, "boss_OMORI_loop");
            _overlay = passive?.Overlay;
        }

        protected override void LateUpdate()
        {
            MusicCheck();
            base.LateUpdate();
        }

        private void MusicCheck()
        {
            if (_overlay == null) return;
            var currentPlayingTheme = SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme;
            _overlay.volume = currentPlayingTheme.volume;
            if (_overlay.isPlaying == currentPlayingTheme.isPlaying || _cmh.LoopSource.isPlaying) return;
            switch (currentPlayingTheme.isPlaying)
            {
                case false:
                    _overlay.Pause();
                    break;
                case true:
                    _overlay.UnPause();
                    break;
            }
        }

        private void OnDestroy()
        {
            Destroy(_overlay);
        }
    }
}