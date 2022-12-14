using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// The base world object type for IP - Could be a ship, stations, asteroid, projectile, wormhole or others
    /// </summary>
    public class ModelUnit
    {
        public int Id { get; set; }

        public int Seed { get; set; } = -1;
        /// <summary>
        /// Defines the type of unit this is
        /// </summary>
        public ModelUnitClass Class { get; set; }
        public ModelSector Sector { get; set; }
        public Vec3 Position { get; set; }
        public Vec3 Rotation { get; set; }
        public ModelFaction Faction { get; set; }
        /// <summary>
        /// Requisition point provision. Provides faction with chance to build more units
        /// </summary>
        public int RpProvision { get; set; }

        /// <summary>
        /// If this unit is a piece of cargo, this will be populated
        /// </summary>
        public ModelUnitCargoData CargoData { get; set; }

        /// <summary>
        /// Not used in 1.6.x
        /// </summary>
        public ModelUnitDebrisData DebrisData { get; set; }

        /// <summary>
        /// Defines info about ships that a station sells
        /// </summary>
        public ModelUnitShipTraderData ShipTraderData { get; set; }

        /// <summary>
        /// Used when this unit is a projectile
        /// </summary>
        public ModelUnitProjectileData ProjectileData { get; set; }

        /// <summary>
        /// Used when this unit is an asteroid
        /// </summary>
        public ModelUnitAsteroidData AsteroidData { get; set; }

        /// <summary>
        /// A custom name for this unit. If null/empty the game displays the unit name based on other rules.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A custom name for this unit. Should only be set where Name is also set
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// This would be set if this is a ship or a station
        /// </summary>
        public ModelComponentUnitData ComponentUnitData { get; set; }

        /// <summary>
        /// Assign to set a custom health value
        /// </summary>
        public ModelUnitHealthData HealthData { get; set; }

        public bool IsInvulnerable { get; set; }
        public bool AvoidDestruction { get; set; }

        /// <summary>
        /// Used for triggers / scripting
        /// </summary>
        public float TotalDamagedReceived { get; set; }

        public List<ModelPassengerGroup> PassengerGroups { get; set; } = new List<ModelPassengerGroup>(10);

        /// <summary>
        /// Set when this unit is a wormhole
        /// </summary>
        public ModelUnitWormholeData WormholeData { get; set; }
        public List<ModelJob> Jobs { get; set; } = new List<ModelJob>();

        /// <summary>
        /// Some data about a ship when it is in the "active" sector.<br />
        /// DO NOT SET THIS IF THE SHIP IS NOT IN THE ACTIVE SECTOR. 
        /// </summary>
        public ModelUnitActiveData ActiveData { get; set; }
        public float? Radius { get; set; }

        /// <summary>
        /// Custom mass if any
        /// </summary>
        public float? Mass { get; set; }

        /// <summary>
        /// Optional - used for custom variants.
        /// </summary>
        public string CustomClassName { get; set; }
    }
}
