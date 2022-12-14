namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders
{
    /// <summary>
    /// Represents an order that the fleet is currently busy with
    /// </summary>
    public class ModelActiveFleetOrder
    {
        public double TimeoutTime { get; set; }
        public double StartTime { get; set; }
        public ModelFleetOrder Order { get; set; }
    }
}
