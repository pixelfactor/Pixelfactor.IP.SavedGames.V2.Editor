using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Represents a single bay inside a hangar, where a ship can dock
    /// </summary>
    public class EditorHangarBay : MonoBehaviour
    {
        /// <summary>
        /// Should be unique for this unit
        /// </summary>
        [Tooltip("Bay ID unique to this hangar - DO NOT CHANGE!")]
        public int BayId = 0;
    }
}
