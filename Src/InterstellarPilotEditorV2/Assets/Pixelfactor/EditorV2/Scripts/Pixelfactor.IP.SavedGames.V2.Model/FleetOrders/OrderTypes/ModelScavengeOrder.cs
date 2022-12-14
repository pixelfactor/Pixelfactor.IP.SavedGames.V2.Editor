﻿using Pixelfactor.IP.Common.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.Models;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes
{
    public class ModelScavengeOrder : ModelFleetOrder
    {
        public CollectCargoOwnerMode CollectOwnerMode { get; set; }
        public ModelSector TargetSector { get; set; }

        public override FleetOrderType OrderType => FleetOrderType.Scavenge;
    }
}
