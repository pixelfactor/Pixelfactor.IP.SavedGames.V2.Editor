namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Fleets settings + home base + formation style
    /// </summary>
    public class ModelPlayerFleetSettingsCombined
    {
        public ModelSectorTarget HomeBase { get; set; }
        public ModelFleetSettings FleetSettings { get; set; }
        public int FormationId { get; set; } = -1;
    }
}
