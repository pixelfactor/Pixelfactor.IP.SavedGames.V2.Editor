using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model.Scenarios;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Data about this specific scenario
    /// </summary>
    public class ModelScenarioData
    {
        public double NextRandomEventTime { get; set; }
        public bool HasRandomEvents { get; set; }
        public ModelFactionSpawner FactionSpawner { get; set; }

        /// <summary>
        /// TODO: This should probably be a mission
        /// </summary>
        public ModelTradeRouteScenarioData TradeRouteScenarioData { get; set; }
        public RespawnOnDeathPreference RespawnOnDeath { get; set; } = RespawnOnDeathPreference.NotSet;
        public bool Permadeath { get; set; } = false;
        public bool AllowTeleporting { get; set; } = true;
        public bool AsteroidRespawningEnabled { get; set; } = true;

        public float AsteroidRespawnTime { get; set; } = 0.5f;
        public float NextProcessOtherEventsTime { get; set; } = 0.0f;

        /// <summary>
        /// Gets or sets if any station can be captured
        /// </summary>
        public bool AllowStationCapture { get; set; } = true;

        /// <summary>
        /// Gets or sets if NPC pilots may abandon their ship during combat
        /// </summary>
        public bool AllowAbandonShip { get; set; } = true;

        /// <summary>
        /// Gets or sets if god mode can be accessed during the scenario
        /// </summary>
        public bool AllowGodMode { get; set; } = true;

        /// <summary>
        /// Gets or sets if the player receives notifications when property is under attack. This can be turned off when the scenario is a skirmish
        /// </summary>
        public bool PlayerPropertyAttackNotifications { get; set; } = true;
    }
}
