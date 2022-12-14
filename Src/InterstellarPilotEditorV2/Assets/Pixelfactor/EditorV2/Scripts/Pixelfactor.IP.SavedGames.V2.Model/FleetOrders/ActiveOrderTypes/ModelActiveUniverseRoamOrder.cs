namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.ActiveOrderTypes
{
    public class ModelActiveUniverseRoamOrder : ModelActiveFleetOrder
    {
        public ModelSector CurrentTargetSector { get; set; }

        /// <summary>
        /// Local to sector
        /// </summary>
        public Vec3 CurrentTargetPosition { get; set; }
    }
}
