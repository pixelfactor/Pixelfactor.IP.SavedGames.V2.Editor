using Pixelfactor.IP.Common;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Represents a type of unit that can hold components/weapons - usually a ship or station
    /// </summary>
    public class EditorComponentUnitData : MonoBehaviour
    {
        public ConstructionState ConstructionState;

        public float ConstructionProgress;

        public float CapacitorCharge = -1.0f;

        public bool IsCloaked;

        public float EngineThrottle = -1.0f;

        /// <summary>
        /// Should be existing within the <see cref="People" collection/>
        /// </summary>
        public EditorPerson Pilot;
    }
}
