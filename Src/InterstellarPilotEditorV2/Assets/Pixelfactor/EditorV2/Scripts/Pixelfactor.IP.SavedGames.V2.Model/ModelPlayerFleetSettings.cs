namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelPlayerFleetSettings
    {
        public bool NotifyWhenOrderComplete { get; set; } = false;
        public bool NotifyWhenScannedHostile { get; set; } = false;
        public bool NotifyWhenAbandonedUnitFound { get; set; } = false;
        public bool NotifyWhenAbandonedCargoFound { get; set; } = false;

        public bool AreSameAs(ModelPlayerFleetSettings modelPlayerFleetSettings)
        {
            return this.NotifyWhenOrderComplete == modelPlayerFleetSettings.NotifyWhenOrderComplete &&
                this.NotifyWhenScannedHostile == modelPlayerFleetSettings.NotifyWhenScannedHostile &&
                this.NotifyWhenAbandonedUnitFound == modelPlayerFleetSettings.NotifyWhenAbandonedUnitFound &&
                this.NotifyWhenAbandonedCargoFound == modelPlayerFleetSettings.NotifyWhenAbandonedCargoFound;
        }
    }
}
