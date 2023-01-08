using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// The base world object type for IP - Could be a ship, stations, asteroid, projectile, wormhole or others
    /// </summary>
    public class EditorUnit : MonoBehaviour
    {
        public int Id = -1;

        /// <summary>
        /// Where a unit is randomized at runtime, you can use this value to get guaranteed data.<br />
        /// Used for: asteroid cluster appearance, gas cloud appearance, unit "designations" e.g. Rations Factory BX-1
        /// </summary>
        public int Seed = -1;

        /// <summary>
        /// Obsolete - use <see cref="EditorCargoClassRef"/> now
        /// </summary>
        [UnityEngine.Serialization.FormerlySerializedAs("Class")]
        public ModelUnitClass LegacyUnitClass = ModelUnitClass.None;

        [UnityEngine.Serialization.FormerlySerializedAs("EditorUnitClassRef")]
        public EditorUnitClassRef UnitClass = null;

        public EditorFaction Faction;
        /// <summary>
        /// Requisition point provision. Provides faction with chance to build more units
        /// </summary>
        public int RpProvision;

        /// <summary>
        /// A custom name for this unit. If null/empty the game displays the unit name based on other rules.
        /// </summary>
        public string Name;

        /// <summary>
        /// Optional custom radius. Only relevant to gas clouds and asteroid clusters
        /// </summary>
        public float Radius = -1.0f;

        void OnDrawGizmosSelected()
        {
            if (SelectionHelper.IsSelected(this) || SelectionHelper.IsSelected(this.transform.parent))
            { 
                DrawString.Draw(this.gameObject.name, this.transform.position, Color.white);
            }
        }

        public ModelUnitClass ModelUnitClass
        {
            get
            {
                if (System.Enum.IsDefined(typeof(ModelUnitClass), this.LegacyUnitClass) && this.LegacyUnitClass != ModelUnitClass.None)
                {
                    return this.LegacyUnitClass;
                }

                if (this.UnitClass != null)
                {
                    return this.UnitClass.UnitClass;
                }

                return ModelUnitClass.None;
            }
        }
    }
}
