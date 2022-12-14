using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// This type was used in early version to artificially inject fleets into the game<br />
    /// It is still used by the <see cref="Entities.Jobs.JobTypes.DestroyFleetJob"/>
    /// </summary>
    public class ModelFleetSpawnParams
    {
        public ModelFaction Faction { get; set; }

        public string FleetResourceName { get; set; }

        /// <summary>
        /// Optional home base for the spawned units
        /// </summary>
        public ModelUnit HomeBaseUnit { get; set; }

        /// <summary>
        /// Optional home sector for the spawned units
        /// </summary>
        public ModelSector HomeSector { get; set; }

        /// <summary>
        /// The designation given to all spawned units
        /// </summary>
        public string ShipDesignation { get; set; }

        /// <summary>
        /// Description of each unit to spawn
        /// </summary>
        public List<ModelFleetSpawnParamsItem> Items = new List<ModelFleetSpawnParamsItem>();

        /// <summary>
        /// Dock that the spawned units will be spawned at if assigned
        /// </summary>
        public ModelUnit TargetDockUnit { get; set; }

        /// <summary>
        /// Position to spawn at (ignored if TargetDock is assigned)
        /// Local to sector
        /// </summary>
        public Vec3 TargetPosition { get; set; }

        /// <summary>
        /// Sector the spawned units spawn in (ignored if TargetDock is assigned)
        /// </summary>
        public ModelSector TargetSector { get; set; }
    }
}
