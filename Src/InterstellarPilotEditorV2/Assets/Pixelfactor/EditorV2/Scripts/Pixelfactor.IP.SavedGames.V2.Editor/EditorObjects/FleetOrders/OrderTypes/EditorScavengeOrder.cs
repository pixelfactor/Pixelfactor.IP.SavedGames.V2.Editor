using Pixelfactor.IP.Common.FleetOrders;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorScavengeOrder : EditorFleetOrderBase
    {
        public EditorSector TargetSector = null;
        /// <summary>
        /// Determines how this order interacts with cargo ownership
        /// </summary>
        public CollectCargoOwnerMode CollectOwnerMode = CollectCargoOwnerMode.OwnedOrNoFaction;
    }
}
