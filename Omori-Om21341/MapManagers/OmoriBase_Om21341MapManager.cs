using CustomMapUtility;
using UnityEngine;
using Util_Om21341.CommonMaps;

namespace Omori_Om21341.MapManagers
{
    public class OmoriBase_Om21341MapManager : OmoriBoomEffectMap_Om21341MapManager
    {
        private EnemyTeamStageManager_Omori_Om21341 _stageManager;
        private AudioClip introClip;
        private bool loop = true;
        private AudioClip loopClip;
        private AudioSource overlay;

        public override void EnableMap(bool b)
        {
            if (!loop && !b)
            {
                Debug.Log("BGM: Map disabled, re-enabling loop");
                SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.loop = true;
                loop = true;
            }

            if (b)
            {
                var currentClip = SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.clip;
                if (currentClip != introClip && currentClip != loopClip)
                    mapBgm = new[] { introClip };
                else
                    mapBgm = new[] { currentClip };
            }

            base.EnableMap(b);
        }

        public override void InitializeMap()
        {
            base.InitializeMap();
            introClip = CustomMapHandler.GetAudioClip("boss_OMORI.ogg");
            loopClip = CustomMapHandler.GetAudioClip("boss_OMORI_loop.ogg");
            _stageManager =
                Singleton<StageController>.Instance.EnemyStageManager as EnemyTeamStageManager_Omori_Om21341;
            overlay = _stageManager?.overlay;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            MusicCheck();
        }
#pragma warning disable IDE0051
        private void Update()
        {
            MusicCheck();
        }
#pragma warning restore IDE0051
        private void MusicCheck()
        {
            if (!isEnabled || !_bMapInitialized) return;
            var currentPlayingTheme = SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme;
            if (introClip == currentPlayingTheme.clip)
            {
                if (!currentPlayingTheme.isPlaying)
                {
                    currentPlayingTheme.clip = loopClip;
                    currentPlayingTheme.Play();
                    mapBgm[0] = loopClip;
                    SingletonBehavior<BattleSoundManager>.Instance.SetEnemyThemeIndexZero(loopClip);
                    Debug.Log("BGM: Exited intro");
                    currentPlayingTheme.loop = true;
                    loop = true;
                    return;
                }

                if (currentPlayingTheme.loop)
                {
                    Debug.Log("BGM: Intro playing, disabling loop");
                    currentPlayingTheme.loop = false;
                    loop = false;
                }

                return;
            }

            if (!loop && !currentPlayingTheme.loop)
            {
                Debug.Log("BGM: Music changed, re-enabling loop");
                currentPlayingTheme.loop = true;
                loop = true;
            }

            if (overlay != null) {
                overlay.volume = currentPlayingTheme.volume;
                if (overlay.isPlaying != currentPlayingTheme.isPlaying) {
                    switch (currentPlayingTheme.isPlaying) {
                        case false:
                            overlay.Pause();
                            break;
                        case true:
                            overlay.UnPause();
                            break;
                    }
                }
            }
        }
#pragma warning disable IDE0051
        private void OnDestroy()
        {
            Destroy(overlay);
        }
#pragma warning restore IDE0051
    }
}