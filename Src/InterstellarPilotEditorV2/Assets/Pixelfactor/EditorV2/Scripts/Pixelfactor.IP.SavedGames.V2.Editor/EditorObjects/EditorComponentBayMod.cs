using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorComponentBayMod : MonoBehaviour
    {
        [Tooltip("The component bay that is being modded")]
        public EditorComponentBayRef ComponentBay;

        [Tooltip("The component that the bay should have")]
        public EditorComponentClass Component;

        /// <summary>
        /// Obsolete - use <see cref="ComponentBay"/> now
        /// </summary>
        [Tooltip("Obsolete and retained for backwards compatibility - use ComponentBay instead")]
        [UnityEngine.Serialization.FormerlySerializedAs("BayId")]
        public int LegacyBayId = 0;

        /// <summary>
        /// Obsolete - use <see cref="EditorComponentClass"/> now
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

        public ModelComponentClass ComponentClass
        {
            get
            {
                if (this.Component != null)
                {
                    return (ModelComponentClass)this.Component.UniqueId;
                }

                return this.LegacyComponentClass;
            }
        }
    }
}
