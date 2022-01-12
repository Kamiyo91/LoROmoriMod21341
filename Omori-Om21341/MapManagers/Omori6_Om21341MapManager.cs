using CustomMapUtility;
using UnityEngine;

namespace Omori_Om21341.MapManagers
{
    public class Omori6_Om21341MapManager : CustomMapManager
    {
        protected override string[] CustomBGMs => new[] { "OmoriPhase2_Om21341.mp3" };

        public override void EnableMap(bool b)
        {
            sephirahColor = Color.black;
            base.EnableMap(b);
        }
    }
}