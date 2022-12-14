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
    }
}
