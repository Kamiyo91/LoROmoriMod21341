using CustomMapUtility;
using UnityEngine;
using Util_Om21341.CommonMaps;

namespace Omori_Om21341.MapManagers
{
    public class OmoriBase_Om21341MapManager : OmoriBoomEffectMap_Om21341MapManager
    {
        private AudioClip _introClip;
        private bool _loop = true;
        private AudioClip _loopClip;
        private AudioSource _overlay;
        private EnemyTeamStageManager_Omori_Om21341 _stageManager;

        public override void EnableMap(bool b)
        {
            if (!_loop && !b)
            {
                Debug.Log("BGM: Map disabled, re-enabling loop");
                SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.loop = true;
                _loop = true;
            }

            if (b)
            {
                var currentClip = SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.clip;
                if (currentClip != _introClip && currentClip != _loopClip)
                    mapBgm = new[] { _introClip };
                else
                    mapBgm = new[] { currentClip };
            }

            base.EnableMap(b);
        }

        public override void InitializeMap()
        {
            base.InitializeMap();
            _introClip = CustomMapHandler.GetAudioClip("boss_OMORI.ogg");
            _loopClip = CustomMapHandler.GetAudioClip("boss_OMORI_loop.ogg");
            _stageManager =
                Singleton<StageController>.Instance.EnemyStageManager as EnemyTeamStageManager_Omori_Om21341;
            _overlay = _stageManager?.Overlay;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            MusicCheck();
        }

        private void Update()
        {
            MusicCheck();
        }

        private void MusicCheck()
        {
            if (!isEnabled || !_bMapInitialized) return;
            var currentPlayingTheme = SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme;
            if (_introClip == currentPlayingTheme.clip)
            {
                if (!currentPlayingTheme.isPlaying)
                {
                    currentPlayingTheme.clip = _loopClip;
                    currentPlayingTheme.Play();
                    mapBgm[0] = _loopClip;
                    SingletonBehavior<BattleSoundManager>.Instance.SetEnemyThemeIndexZero(_loopClip);
                    Debug.Log("BGM: Exited intro");
                    currentPlayingTheme.loop = true;
                    _loop = true;
                    return;
                }

                if (!currentPlayingTheme.loop) return;
                Debug.Log("BGM: Intro playing, disabling loop");
                currentPlayingTheme.loop = false;
                _loop = false;

                return;
            }

            if (!_loop && !currentPlayingTheme.loop)
            {
                Debug.Log("BGM: Music changed, re-enabling loop");
                currentPlayingTheme.loop = true;
                _loop = true;
            }

            if (_overlay == null) return;
            _overlay.volume = currentPlayingTheme.volume;
            if (_overlay.isPlaying == currentPlayingTheme.isPlaying) return;
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