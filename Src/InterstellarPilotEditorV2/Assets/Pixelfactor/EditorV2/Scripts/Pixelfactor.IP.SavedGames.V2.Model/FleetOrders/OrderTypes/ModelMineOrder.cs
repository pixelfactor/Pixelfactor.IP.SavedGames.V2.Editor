﻿using Pixelfactor.IP.Common.FleetOrders;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes
{
    public class ModelMineOrder : ModelFleetOrder
    {
        public ModelSector TargetSector { get; set; }
        public CollectCargoOwnerMode CollectOwnerMode { get; set; }
        public ModelUnit ManualMineTarget { get; set; }

        public override FleetOrderType OrderType => FleetOrderType.Mine;
    }
}
