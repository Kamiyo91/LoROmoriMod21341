using UnityEngine;

namespace Omori_Om21341.MapManagers
{
    public class Omori1_Om21341MapManager : OmoriBase_Om21341MapManager
    {
        protected override string[] CustomBGMs => new[] { "boss_OMORI.ogg" };
        public override void EnableMap(bool b)
        {
            sephirahColor = Color.black;
            base.EnableMap(b);
        }
    }
}