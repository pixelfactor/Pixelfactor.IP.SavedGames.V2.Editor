using Pixelfactor.IP.Common.FleetOrders;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorManualRepairFleetOrder : EditorFleetOrderBase
    {
        public InsufficientCreditsMode InsufficientCreditsMode = InsufficientCreditsMode.Wait;
        public EditorUnit RepairLocationUnit;
    }
}
