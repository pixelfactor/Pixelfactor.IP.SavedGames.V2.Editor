using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines a cargo container that can be tractored by a ship
    /// </summary>
    public class EditorCargoUnit : MonoBehaviour
    {
        /// <summary>
        /// Obsolete - avoid use and switch over to <see cref="CargoClass"/>
        /// </summary>
        [UnityEngine.Serialization.FormerlySerializedAs("CargoClass")]
        public ModelCargoClass LegacyCargoClass;

        [UnityEngine.Serialization.FormerlySerializedAs("CargoClassRef")]
        public EditorCargoClassRef CargoClass;

        public int Quantity = 1;
        public bool Expires = false;
        public double SpawnTime = 0d;

        public ModelCargoClass ModelCargoClass
        {
            get
            {
                if (System.Enum.IsDefined(typeof(ModelCargoClass), this.LegacyCargoClass) && this.LegacyCargoClass != ModelCargoClass.None)
                {
                    return this.LegacyCargoClass;
                }

                if (this.CargoClass != null)
                {
                    return this.CargoClass.CargoClass;
                }

                return ModelCargoClass.None;
            }
        }
    }
}
