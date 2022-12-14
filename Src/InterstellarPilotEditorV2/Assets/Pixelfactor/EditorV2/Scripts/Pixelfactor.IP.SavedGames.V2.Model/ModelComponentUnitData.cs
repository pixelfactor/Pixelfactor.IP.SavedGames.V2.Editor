using Pixelfactor.IP.Common;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Represents a type of unit that can hold components/weapons - usually a ship or station
    /// </summary>
    public class ModelComponentUnitData
    {
        /// <summary>
        /// Unit cannnot be captured until after this time
        /// </summary>
        public double? CaptureCooldownTime { get; set; }

        /// <summary>
        /// If using a generated ship name this would be assigned
        /// </summary>
        public string CustomShipName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float? CargoCapacity { get; set; }

        /// <summary>
        /// Custom scan range
        /// </summary>
        public int? ScanRange { get; set; }

        public ModelComponentUnitFactoryData FactoryData { get; set; }

        public ConstructionState ConstructionState { get; set; }

        public float ConstructionProgress { get; set; } = 1.0f;

        /// <summary>
        /// Used when the ship/station has different components from the vanilla spec
        /// </summary>
        public ModelComponentUnitModData ModData { get; set; }

        /// <summary>
        /// 0 - 1
        /// </summary>
        public float? CapacitorCharge { get; set; }

        public bool IsCloaked { get; set; }

        public List<int> PoweredDownBayIds { get; set; } = new List<int>();

        public List<int> AutoFireBayIds { get; set; } = new List<int>();

        public float? EngineThrottle { get; set; }

        public ModelComponentUnitCargoData CargoData { get; set; }

        public ModelComponentUnitShieldHealthData ShieldData { get; set; }

        public ModelComponentUnitComponentHealthData ComponentHealthData { get; set; }

        /// <summary>
        /// Ships docked at this unit
        /// </summary>
        public ModelComponentUnitDockData DockData { get; set; }

        /// <summary>
        /// Should be existing within the <see cref="People" collection/>
        /// </summary>
        public ModelPerson Pilot { get; set; }

        /// <summary>
        /// All the people on this ship/station
        /// </summary>
        public List<ModelPerson> People { get; set; } = new List<ModelPerson>();
        public int ShipNameIndex { get; set; }

        public AutoTurretFireMode? AutoTurretFireMode { get; set; } = null;
    }
}
