using System.Collections.Generic;
using System.IO;
using CustomMapUtility;
using KamiyoStaticBLL.Models;
using KamiyoStaticUtil.Utils;
using UnityEngine;

namespace Util_Om21341
{
    public static class MapUtil
    {
        public static void ChangeMap(MapModel model, Faction faction = Faction.Player)
        {
            if (MapStaticUtil.CheckStageMap(model.StageIds) || SingletonBehavior<BattleSceneRoot>
                    .Instance.currentMapObject.isEgo ||
                Singleton<StageController>.Instance.GetStageModel().ClassInfo.stageType == StageType.Creature) return;
            CustomMapHandler.InitCustomMap(model.Stage, model.Component, model.IsPlayer, model.InitBgm, model.Bgx,
                model.Bgy, model.Fx, model.Fy);
            if (model.IsPlayer && !model.OneTurnEgo)
            {
                CustomMapHandler.ChangeToCustomEgoMapByAssimilation(model.Stage, faction);
                return;
            }

            CustomMapHandler.ChangeToCustomEgoMap(model.Stage, faction);
            MapStaticUtil.MapChangedValue(true);
        }

        public static void ReturnFromEgoMap(string mapName, List<LorId> ids, bool isAssimilationMap = false)
        {
            if (MapStaticUtil.CheckStageMap(ids) ||
                Singleton<StageController>.Instance.GetStageModel().ClassInfo.stageType ==
                StageType.Creature) return;
            CustomMapHandler.RemoveCustomEgoMapByAssimilation(mapName);
            MapStaticUtil.RemoveValueInAddedMap(mapName);
            if (!isAssimilationMap) return;
            MapStaticUtil.MapChangedValue(true);
            if (!string.IsNullOrEmpty(Singleton<StageController>.Instance.GetStageModel().GetCurrentMapInfo()))
                CustomMapHandler.EnforceTheme();
            Singleton<StageController>.Instance.CheckMapChange();
            SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(SingletonBehavior<BattleSceneRoot>
                .Instance.currentMapObject.mapBgm);
            SingletonBehavior<BattleSoundManager>.Instance.CheckTheme();
        }

        public static void GetArtWorks(DirectoryInfo dir)
        {
            if (dir.GetDirectories().Length != 0)
            {
                var directories = dir.GetDirectories();
                foreach (var t in directories) GetArtWorks(t);
            }

            foreach (var fileInfo in dir.GetFiles())
            {
                var texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(File.ReadAllBytes(fileInfo.FullName));
                var value = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),
                    new Vector2(0f, 0f));
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                ModParameters.ArtWorks[fileNameWithoutExtension] = value;
            }
        }
    }
}