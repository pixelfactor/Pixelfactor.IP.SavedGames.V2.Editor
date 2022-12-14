using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Used by some scenarios to have fleets patrol, mainly because the at the time of creating the scenarios, factions could not generate patrol routes dynamically.<br />
    /// This mechanism is not used when generating a custom universe, though there is no reason why it couldn't be.
    /// </summary>
    public class ModelSectorPatrolPath
    {
        public int Id { get; set; }
        public ModelSector Sector { get; set; }
        public bool IsLoop { get; set; }

        public List<ModelSectorPatrolPathNode> Nodes { get; set; } = new List<ModelSectorPatrolPathNode>();
    }
}
