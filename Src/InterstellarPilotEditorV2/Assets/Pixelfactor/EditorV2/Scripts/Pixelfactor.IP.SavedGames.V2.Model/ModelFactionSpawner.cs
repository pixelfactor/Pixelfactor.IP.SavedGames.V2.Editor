using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelFactionSpawner
    {
        public double NextFactionSpawnTime { get; set; } = -1.0;

        public double NextFreelancerSpawnTime { get; set; } = -1.0;

        public List<ModelFactionTypeSpawnSetting> FactionTypeSpawnSettings { get; set; } = new List<ModelFactionTypeSpawnSetting>();
    }
}
