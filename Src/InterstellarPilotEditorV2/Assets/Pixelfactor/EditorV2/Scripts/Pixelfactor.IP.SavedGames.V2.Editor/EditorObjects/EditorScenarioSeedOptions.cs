using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Allows the game engine to be used to seed certain things when playing the scenario for the first time
    /// </summary>
    public class EditorScenarioSeedOptions : MonoBehaviour
    {
        public bool SeedUnstableWormholes = false;
        public bool SeedPlanets = false;

        public bool SeedAsteroidClusters = false;

        public bool SeedAsteroids = false;

        public bool SeedSprinkledAsteroids = false;

        public bool SeedGasClouds = false;

        public bool SeedEmpireFactions = false;

        public bool SeedBanditFactions = false;

        public bool SeedBars = false;

        /// <summary>
        /// Whether to seed factions other than bandits and empires
        /// </summary>
        public bool SeedOtherFactions = false;

        /// <summary>
        /// Whether to give factions intel on the universe
        /// </summary>
        public bool SeedFactionStaticIntel = false;

        /// <summary>
        /// Whether to give factions intel of other faction's units
        /// </summary>
        public bool SeedFactionUnitIntel = false;

        /// <summary>
        /// Whether to give factions intel of other factions
        /// </summary>
        public bool SeedFactionOnFactionIntel = false;

        /// <summary>
        /// Whether to give factions intel of other factions
        /// </summary>
        public bool SeedFactionRelations = false;

        public bool SeedModdedUnits = false;
        public bool SeedPassengerGroups = false;
        public bool SeedDepletedAsteroids = false;

        public bool SeedRandomizedFleets = false;
        public bool SeedBounty = false;
        /// <summary>
        /// Determines whether at the start of a new game, cargo will be added to unit traders based on their preferred stock levels
        /// </summary>
        public bool SeedTraderCargo = false;
        public bool SeedShipDamage = false;

        public bool SeedSmallAsteroids = false;
        public bool SeedAbandonedShips = false;
        public bool SeedAbandonedCargo = false;

        public bool SeedPilotPromotions = false;
    }
}
