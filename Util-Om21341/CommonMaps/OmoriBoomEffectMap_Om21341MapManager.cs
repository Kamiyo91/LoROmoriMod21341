using CustomMapUtility;
using OmoriMod_Om21341.BLL_Om21341;
using UnityEngine;

namespace OmoriMod_Om21341.Util_Om21341.CommonMaps
{
    public class OmoriBoomEffectMap_Om21341MapManager : CustomMapManager
    {
        private GameObject _aura;

        public override void InitializeMap()
        {
            base.InitializeMap();
            if (OmoriModParameters.BoomEffectMap == null) CommonUtil.LoadBoomEffect();
        }

        public override void EnableMap(bool b)
        {
            sephirahColor = Color.black;
            base.EnableMap(b);
        }

        public static void BoomFirst()
        {
            var gameObject = Instantiate(OmoriModParameters.BoomEffectMap.areaBoomEffect);
            var battleUnitModel = BattleObjectManager.instance.GetList(Faction.Enemy)[0];
            gameObject.transform.SetParent(battleUnitModel.view.gameObject.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            gameObject.AddComponent<AutoDestruct>().time = 4f;
            gameObject.SetActive(true);
        }

        public void BoomSecond()
        {
            BoomFirst();
            DestroyAura();
        }

        private void DestroyAura()
        {
            if (_aura == null) return;
            Destroy(_aura);
            _aura = null;
        }
    }
}