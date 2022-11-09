using UnityEngine;

namespace OmoriMod_Om21341.Omori_Om21341.MapManagers
{
    public class Omori5_Om21341MapManager : OmoriBase_Om21341MapManager
    {
        public override void EnableMap(bool b)
        {
            sephirahColor = Color.black;
            base.EnableMap(b);
        }
    }
}