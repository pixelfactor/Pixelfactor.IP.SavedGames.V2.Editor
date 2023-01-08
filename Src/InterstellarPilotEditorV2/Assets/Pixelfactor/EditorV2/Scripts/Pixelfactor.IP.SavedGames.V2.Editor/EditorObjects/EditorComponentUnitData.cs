using Pixelfactor.IP.Common;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Represents a type of unit that can hold components/weapons - usually a ship or station
    /// </summary>
    public class EditorComponentUnitData : MonoBehaviour
    {
        [Tooltip("Whether the unit is being constructed or dismantled. Applies to ships and stations")]
        public ConstructionState ConstructionState = ConstructionState.Constructed;

        [Tooltip("Determines how far the unit has been constructed. Note that this value decreases to zero when being dismantled")]
        [Range(0.0f, 1.0f)]
        public float ConstructionProgress = 1.0f;

        [Tooltip("Optional charge of the capacitor in the range of 0 to 1 (100%)")]
        public float CapacitorCharge = -1.0f;

        [Tooltip("Whether the unit is currently fully cloaked")]
        public bool IsCloaked = false;

        [Range(0.0f, 1.0f)]
        [Tooltip("Optional current throttle of the unit")]
        public float EngineThrottle = 0.0f;

        /// <summary>
        /// Should be existing within the <see cref="People" collection/>
        /// </summary>
        [Tooltip("Optional current pilot of the unit. Note that an NPC that is a child of a ship will automatically be assumed to be the pilot")]
        public EditorPerson Pilot;

        [Tooltip("Optional custom cargo capacity in cargo units. For units that have a cargo bay")]
        public float CargoCapacity = -1.0f;
    }
}
