namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelSectorTarget
    {
        /// <summary>
        /// Local position to sector
        /// </summary>
        public Vec3 Position { get; set; }

        public ModelSector Sector { get; set; }

        public ModelUnit TargetUnit { get; set; }

        public ModelFleet TargetFleet { get; set; }

        /// <summary>
        /// True when the order had a valid target unit or target fleet
        /// </summary>
        public bool HadValidTarget { get; set; }
    }
}
