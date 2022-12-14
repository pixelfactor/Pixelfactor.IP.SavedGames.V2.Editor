using Pixelfactor.IP.Common.FleetOrders;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes
{
    public class ModelUniverseTradeOrder : ModelFleetOrder
    {
        public int MinBuyQuantity { get; set; }
        public float MinBuyCargoPercentage { get; set; }
        public bool TradeOnlySpecificCargoClasses { get; set; }

        /// <summary>
        /// If trading only certain cargo, these are the ids
        /// TODO: Change id to enum
        /// </summary>
        public List<ModelCargoClass> TradeSpecificCargoClasses { get; set; } = new List<ModelCargoClass>();

        public override FleetOrderType OrderType => FleetOrderType.AutonomousTrade;
    }
}
