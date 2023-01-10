using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorComponentBayMod : MonoBehaviour
    {
        /// <summary>
        /// Obsolete - use <see cref="ComponentBay"/> now
        /// </summary>
        [Tooltip("Obsolete and retained for backwards compatibility - use ComponentBay instead")]
        [UnityEngine.Serialization.FormerlySerializedAs("BayId")]
        public int LegacyBayId = 0;

        [Tooltip("The component bay that is being modded")]
        public EditorComponentBayRef ComponentBay;

        /// <summary>
        /// Obsolete - use <see cref="ComponentClass"/> now
        /// </summary>
        [Tooltip("Obsolete and retained for backwards compatibility - use ComponentClass instead")]
        [UnityEngine.Serialization.FormerlySerializedAs("ModdedComponentClass")]
        public ModelComponentClass LegacyComponentClass = (ModelComponentClass)(-1);

        public int ComponentBayId
        {
            get
            {
                if (this.ComponentBay != null)
                {
                    return this.ComponentBay.BayId;
                }

                return this.LegacyBayId;
            }
        }
    }
}
