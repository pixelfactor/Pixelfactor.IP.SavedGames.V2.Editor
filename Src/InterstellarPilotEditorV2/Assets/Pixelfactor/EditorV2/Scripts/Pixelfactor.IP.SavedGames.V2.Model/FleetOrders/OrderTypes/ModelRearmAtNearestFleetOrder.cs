﻿using Pixelfactor.IP.Common.FleetOrders;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes
{
    public class ModelRearmAtNearestFleetOrder : ModelFleetOrder
    {
        public InsufficientCreditsMode InsufficientCreditsMode { get; set; }

        /// <summary>
        /// Gets or sets the percentage of a ships cargo space that should be taken up with equipment
        /// </summary>
        public float EquipmentCargoUsage { get; set; } = 0.25f;

        public override FleetOrderType OrderType => FleetOrderType.RearmAtNearest;
    }
}
