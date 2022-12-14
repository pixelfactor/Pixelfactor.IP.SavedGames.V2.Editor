using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders
{
    public class ModelFleetOrderCollection
    {
        /// <summary>
        /// This collection should contain both queued, current and inactive objectives. Some scenarios give fleets disabled objectives that are later enabled.
        /// </summary>
        public List<ModelFleetOrder> Orders { get; set; } = new List<ModelFleetOrder>();

        public List<ModelFleetOrder> QueuedOrders { get; set; } = new List<ModelFleetOrder>();

        /// <summary>
        /// What the fleet is currently doing.. move to, dock etc
        /// </summary>
        public ModelActiveFleetOrder CurrentOrder { get; set; }
    }
}
