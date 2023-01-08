using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines an item of cargo on a ship/station
    /// </summary>
    public class EditorComponentUnitCargoDataItem : MonoBehaviour
    {
        [Tooltip("The type of the cargo")]
        public ModelCargoClass CargoClass;

        [Tooltip("The amount of this cargo")]
        public int Quantity = 1;
    }
}
