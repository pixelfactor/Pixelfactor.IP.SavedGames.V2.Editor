﻿using Pixelfactor.IP.Common.FleetOrders;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes
{
    public class ModelExploreSectorOrder : ModelFleetOrder
    {
        /// <summary>
        /// The sector to explore
        /// </summary>
        public ModelSector Sector { get; set; }

        public override FleetOrderType OrderType => FleetOrderType.ExploreSector;
    }
}
