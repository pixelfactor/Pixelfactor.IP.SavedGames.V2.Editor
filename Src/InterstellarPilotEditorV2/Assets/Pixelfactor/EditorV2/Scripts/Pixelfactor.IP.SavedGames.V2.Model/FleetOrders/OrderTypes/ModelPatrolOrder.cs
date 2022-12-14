﻿using Pixelfactor.IP.Common.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.Models;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes
{
    public class ModelPatrolOrder : ModelFleetOrder
    {
        public int PathDirection { get; set; }
        public bool IsLooping { get; set; }
        public List<ModelPatrolPathNode> Nodes { get; set; } = new List<ModelPatrolPathNode>();
        public bool IsLoop { get; set; }

        public override FleetOrderType OrderType => FleetOrderType.Patrol;
    }
}
