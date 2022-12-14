namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Sector/room within the universe
    /// </summary>
    public class ModelSector
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Determines where the sector appears on the universe map
        /// </summary>
        public Vec3 MapPosition { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Determines the distance of the wormholes from the sector origin. Should be ~1.0
        /// </summary>
        public float GateDistanceMultiplier { get; set; }

        /// <summary>
        /// Something to do with the appearance of asteroid clusters
        /// </summary>
        public int RandomSeed { get; set; }

        /// <summary>
        /// Skybox rotation
        /// </summary>
        public Vec3 BackgroundRotation { get; set; }

        /// <summary>
        /// Single directional light rotation
        /// </summary>
        public Vec3 DirectionLightRotation { get; set; }
        public Vec3 DirectionLightColor { get; set; }
        public Vec3 AmbientLightColor { get; set; }

        /// <summary>
        /// Game time when the sector's controlling faction changed
        /// </summary>
        public double LastTimeChangedControl { get; set; }

        public float LightDirectionFudge { get; set; }

        /// <summary>
        /// Optional settings to give sector custom sky background
        /// </summary>
        public ModelSectorAppearance CustomAppearance { get; set; }
    }
}
