using Pixelfactor.IP.Common.Factions;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelFactionTypeSpawnSetting
    {
        /// <summary>
        /// The type(s) of faction that this spawn setting affects
        /// </summary>
        public FactionType FactionType;

        /// <summary>
        /// The size of faction that this spawn setting affects 
        /// </summary>
        public FactionFreelancerType FreelancerType;

        /// <summary>
        /// Whether this faction type can spawn
        /// </summary>
        public bool AllowSpawn;
    }
}
