using UnityEngine;

namespace Omori_Om21341.MapManagers
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