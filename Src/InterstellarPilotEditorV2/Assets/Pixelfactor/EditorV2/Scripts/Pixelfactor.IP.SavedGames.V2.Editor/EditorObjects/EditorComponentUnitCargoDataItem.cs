using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines an item of cargo on a ship/station
    /// </summary>
    public class EditorComponentUnitCargoDataItem : MonoBehaviour
    {
        [UnityEngine.Serialization.FormerlySerializedAs("CargoClass")]
        [Tooltip("The type of the cargo. Obsolete - assign CargoClass instead")]
        public ModelCargoClass LegacyCargoClass;

        [Tooltip("The amount of this cargo")]
        public int Quantity = 1;

        [Tooltip("The type of the cargo")]
        public EditorCargoClassRef Cargo = null;

        public ModelCargoClass ModelCargoClass
        {
            get
            {
                if (this.Cargo != null)
                {
                    return this.Cargo.CargoClass;
                }

                return this.LegacyCargoClass;
            }
        }
    }
}
