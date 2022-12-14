using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorDockOrder : EditorFleetOrderBase
    {
        public EditorUnit TargetDock;
    }
}
