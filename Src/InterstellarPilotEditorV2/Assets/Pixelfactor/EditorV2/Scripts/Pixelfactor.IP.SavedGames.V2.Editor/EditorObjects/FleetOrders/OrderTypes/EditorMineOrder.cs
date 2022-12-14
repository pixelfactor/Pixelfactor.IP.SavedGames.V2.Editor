using Pixelfactor.IP.Common.FleetOrders;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorMineOrder : EditorFleetOrderBase
    {
        public CollectCargoOwnerMode CollectOwnerMode = CollectCargoOwnerMode.OwnedOrNoFaction;

        /// <summary>
        /// Optionally set this to a specific asteroid
        /// </summary>
        public EditorUnit ManualMineTarget;
        public EditorSector TargetSector;
    }
}
