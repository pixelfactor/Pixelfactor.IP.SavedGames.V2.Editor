namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelSeedOptions
    {
        public bool SeedUnstableWormholes { get; set; } = false;
        public bool SeedPlanets { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to seed new asteroid clusters
        /// </summary>
        public bool SeedAsteroidClusters { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to seed asteroids inside asteroid clusters
        /// </summary>
        public bool SeedAsteroids { get; set; } = false;
        /// <summary>
        /// Gets or sets whether to seed asteroids outside of asteroid clusters
        /// </summary>
        public bool SeedSprinkledAsteroids { get; set; } = false;
        public bool SeedGasClouds { get; set; } = false;

        public bool SeedEmpireFactions { get; set; } = false;
        public bool SeedBanditFactions { get; set; } = false;
        public bool SeedBars { get; set; } = false;
        /// <summary>
        /// Whether to seed factions other than bandits and empires
        /// </summary>
        public bool SeedOtherFactions { get; set; } = false;

        /// <summary>
        /// Whether to seed faction discover of sectors/asteroids/wormholes
        /// </summary>
        public bool SeedFactionStaticIntel { get; set; } = false;

        /// <summary>
        /// Whether to seed faction discovery of ships/stations
        /// </summary>
        public bool SeedFactionUnitIntel { get; set; } = false;

        /// <summary>
        /// Whether to seed faction discovery of other factions
        /// </summary>
        public bool SeedFactionOnFactionIntel { get; set; } = false;

        public bool SeedFactionRelations { get; set; } = false;

        public bool SeedModdedUnits { get; set; } = false;
        public bool SeedPassengerGroups { get; set; } = false;
        public bool SeedDepletedAsteroids { get; set; } = false;
        public bool SeedTraderCargo { get; set; } = false;
        public bool SeedShipDamage { get; set; } = false;

        public bool SeedRandomizedFleets { get; set; } = false;
        public bool SeedBounty { get; set; } = false;

        public bool SeedSmallAsteroids { get; set; } = false;
        public bool SeedAbandonedShips { get; set; } = false;
        public bool SeedAbandonedCargo { get; set; } = false;

        public bool SeedPilotPromotions { get; set; } = false;
    }
}
