namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.ActiveOrderTypes
{
    public class ModelActiveScavengeOrder : ModelActiveFleetOrder
    {
        //public Unit TractorTargetUnit { get; set; }

        //public bool IsRoaming { get; set; }
        public Vec3? Position { get; set; }

        ///// <summary>
        ///// Local to sector
        ///// </summary>
        //public Vec3 LastKnownCargoPosition { get; set; }
        //public bool HadCargoTarget { get; set; }
    }
}
