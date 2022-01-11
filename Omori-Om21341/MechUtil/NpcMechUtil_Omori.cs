using BLL_Om21341.Models;
using BLL_Om21341.Models.MechUtilModels;
using Util_Om21341.BaseClass;

namespace Omori_Om21341.MechUtil
{
    public class NpcMechUtil_Omori : NpcMechUtilBase
    {
        private readonly NpcMechUtilBaseModel _model;
        private int _phase;
        private bool _singleUse;

        public NpcMechUtil_Omori(NpcMechUtilBaseModel model) : base(model)
        {
            _model = model;
        }

        public int GetPhase()
        {
            return _phase;
        }

        public void IncreasePhase()
        {
            _phase++;
        }

        public override void OnSelectCardPutMassAttack(ref BattleDiceCardModel origin)
        {
            if (_phase == 1 && !_singleUse)
            {
                _singleUse = true;
                origin = BattleDiceCardModel.CreatePlayingCard(
                    ItemXmlDataList.instance.GetCardItem(new LorId(ModParameters.PackageId, 907)));
                return;
            }

            if (_phase <= 1 || _model.OneTurnCard) return;
            SetOneTurnCard(true);
            origin = BattleDiceCardModel.CreatePlayingCard(
                ItemXmlDataList.instance.GetCardItem(new LorId(ModParameters.PackageId, 907)));
        }
    }
}