using CustomMapUtility;
using UnityEngine;
using Util_Om21341.CommonMaps;

namespace Omori_Om21341.MapManagers
{
    public class OmoriBase_Om21341MapManager : OmoriBoomEffectMap_Om21341MapManager
    {
        private AudioClip introClip;
        private AudioClip loopClip;
        private EnemyTeamStageManager_Omori_Om21341 _stageManager;
        private bool loop = true;
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
            _stageManager = Singleton<StageController>.Instance.EnemyStageManager as EnemyTeamStageManager_Omori_Om21341;
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
            if (introClip == SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.clip)
            {
                if (!SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.isPlaying)
                {
                    SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.clip = loopClip;
                    SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.Play();
                    mapBgm[0] = loopClip;
                    SingletonBehavior<BattleSoundManager>.Instance.SetEnemyThemeIndexZero(loopClip);
                    Debug.Log("BGM: Exited intro");
                    SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.loop = true;
                    loop = true;
                    return;
                }

                if (SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.loop)
                {
                    Debug.Log("BGM: Intro playing, disabling loop");
                    SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.loop = false;
                    loop = false;
                }

                return;
            }

            if (!loop && !SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.loop)
            {
                Debug.Log("BGM: Music changed, re-enabling loop");
                SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme.loop = true;
                loop = true;
            }

            if (overlay != null) overlay.volume = SingletonBehavior<BattleSoundManager>.Instance.VolumeBGM;
        }
#pragma warning disable IDE0051
        private void OnDestroy()
        {
            Destroy(overlay);
        }
#pragma warning restore IDE0051
    }
}