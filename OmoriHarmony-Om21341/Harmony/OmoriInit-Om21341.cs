using System;
using System.IO;
using System.Reflection;
using BLL_Om21341.Models;
using Util_Om21341;

namespace OmoriHarmony_Om21341.Harmony
{
    public class OmoriInit_Om21341 : ModInitializer
    {
        public override void OnInitializeMod()
        {
            ModParameters.Path = Path.GetDirectoryName(
                Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            new HarmonyLib.Harmony("LOR.OmoriModOm21341_MOD").PatchAll();
            ModParameters.Language = GlobalGameManager.Instance.CurrentOption.language;
            MapUtil.GetArtWorks(new DirectoryInfo(ModParameters.Path + "/ArtWork"));
            UnitUtil.ChangeCardItem(ItemXmlDataList.instance);
            UnitUtil.ChangePassiveItem();
            SkinUtil.PreLoadBufIcons();
            LocalizeUtil.AddLocalize();
            LocalizeUtil.RemoveError();
        }
    }
}