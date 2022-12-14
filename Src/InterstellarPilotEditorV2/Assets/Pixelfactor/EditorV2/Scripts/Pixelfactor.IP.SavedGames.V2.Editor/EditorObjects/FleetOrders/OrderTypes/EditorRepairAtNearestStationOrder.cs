using Pixelfactor.IP.Common.FleetOrders;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorRepairAtNearestStationOrder : EditorFleetOrderBase
    {
        public InsufficientCreditsMode InsufficientCreditsMode = InsufficientCreditsMode.Wait;
    }
}
