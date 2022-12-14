using Pixelfactor.IP.Common.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelFleet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Position local to sector
        /// </summary>

        public Vec3 Position { get; set; }

        public Vec4 Rotation { get; set; }

        public ModelSector Sector { get; set; }

        public ModelFaction Faction { get; set; }

        public ModelSectorTarget HomeBase { get; set; }

        /// <summary>
        /// When true the parent faction won't' try to control
        /// </summary>
        public bool ExcludeFromFactionAI { get; set; }

        /// <summary>
        /// Optional custom settings
        /// </summary>
        public ModelFleetSettings FleetSettings { get; set; }

        /// <summary>
        /// Note: Due to a massive bug in the way 1.6.x loads a save game, fleet orders in the save file can be ignored when game loads <em>if the order involves another fleet</em><br />
        /// E.g. The attack fleet order will sometimes break when loaded
        /// </summary>
        public ModelFleetOrderCollection OrdersCollection { get; set; } = new ModelFleetOrderCollection();

        public bool IsActive { get; set; } = true;

        /// <summary>
        /// The npcs inside this fleet (pilots)
        /// </summary>
        public List<ModelNpcPilot> Npcs { get; set; } = new List<ModelNpcPilot>();
        public int Seed { get; set; } = -1;
        public FactionStrategy Strategy { get; set; } = FactionStrategy.Unspecified;

        public int FormationId { get; set; } = -1;
    }
}
