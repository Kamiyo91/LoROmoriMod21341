using System;
using System.IO;
using System.Reflection;
using OmoriMod_Om21341.BLL_Om21341;

namespace OmoriMod_Om21341.OmoriHarmony_Om21341.Harmony
{
    public class OmoriInit_Om21341 : ModInitializer
    {
        public override void OnInitializeMod()
        {
            OmoriModParameters.Path = Path.GetDirectoryName(
                Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
        }
    }
}