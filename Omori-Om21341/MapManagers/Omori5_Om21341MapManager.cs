using UnityEngine;
using Util_Om21341.CustomMapUtility.Assemblies;

namespace Omori_Om21341.MapManagers
{
    public class Omori5_Om21341MapManager : CustomMapManager
    {
        protected override string[] CustomBGMs => new[] { "OmoriPhase1_Om21341.mp3" };

        public override void EnableMap(bool b)
        {
            sephirahColor = Color.black;
            base.EnableMap(b);
        }
    }
}