using Pixelfactor.IP.Common.FleetOrders;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorCollectCargoOrder : EditorFleetOrderBase
    {
        public EditorCargoUnit CargoUnit = null;
    }
}
