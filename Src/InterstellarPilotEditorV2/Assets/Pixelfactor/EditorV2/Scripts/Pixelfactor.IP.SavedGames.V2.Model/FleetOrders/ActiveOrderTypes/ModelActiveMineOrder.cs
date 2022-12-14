﻿using Pixelfactor.IP.Common.FleetOrders;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.ActiveOrderTypes
{
    public class ModelActiveMineOrder : ModelActiveFleetOrder
    {
        public ModelUnit MineTarget { get; set; }
        public ActiveMineOrderState State { get; set; }
        public float AngleFromAsteroid { get; set; }
        public float DistanceFromAsteroid { get; set; }
    }
}
