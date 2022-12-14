using Pixelfactor.IP.Common.Factions;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelFactionRelationDataItem
    {
        public ModelFaction OtherFaction { get; set; }

        public bool PermanentPeace { get; set; }

        public bool RestrictHostilityTimeout { get; set; }
        public Neutrality Neutrality { get; set; }

        /// <summary>
        /// Fixed time when make peace
        /// </summary>
        public double HostilityEndTime { get; set; }
    }
}
