using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorWaitOrder : EditorFleetOrderBase
    {
        public float WaitTime = 120f;
    }
}
