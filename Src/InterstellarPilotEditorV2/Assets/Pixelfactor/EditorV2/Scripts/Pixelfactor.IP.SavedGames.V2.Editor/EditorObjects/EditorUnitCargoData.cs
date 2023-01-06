using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorUnitCargoData : MonoBehaviour
    {
        /// <summary>
        /// Obsolete - avoid use and switch over to <see cref="CargoClassRef"/>
        /// </summary>
        [UnityEngine.Serialization.FormerlySerializedAs("CargoClass")]
        public ModelCargoClass LegacyCargoClass;

        public EditorCargoClassRef CargoClassRef;
        public int Quantity = 1;
        public bool Expires = false;
        public double SpawnTime = 0d;

        public ModelCargoClass ModelCargoClass
        {
            get
            {
                if (System.Enum.IsDefined(typeof(ModelCargoClass), this.LegacyCargoClass))
                {
                    return this.LegacyCargoClass;
                }

                if (this.CargoClassRef != null)
                {
                    return this.CargoClassRef.CargoClass;
                }

                return ModelCargoClass.None;
            }
        }
    }
}
