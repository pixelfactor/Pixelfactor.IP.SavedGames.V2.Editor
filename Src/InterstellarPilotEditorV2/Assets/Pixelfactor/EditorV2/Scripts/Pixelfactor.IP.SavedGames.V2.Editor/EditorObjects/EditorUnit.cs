using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// The base world object type for IP - Could be a ship, stations, asteroid, projectile, wormhole or others
    /// </summary>
    public class EditorUnit : MonoBehaviour
    {
        [Tooltip("An id that is unique to all units. If the value is less than zero (the default) a unique id will be automatically assigned when exporting")]
        public int Id = -1;

        /// <summary>
        /// Where a unit is randomized at runtime, you can use this value to get guaranteed data.<br />
        /// Used for: asteroid cluster appearance, gas cloud appearance, unit "designations" e.g. Rations Factory BX-1
        /// </summary>
        [Tooltip("Where a unit is randomized at runtime, you can use this value to get guaranteed data. Used for: asteroid cluster appearance, gas cloud appearance, unit 'designations' e.g. Rations Factory BX-1")]
        public int Seed = -1;

        /// <summary>
        /// Obsolete - use <see cref="EditorCargoClassRef"/> now
        /// </summary>
        [UnityEngine.Serialization.FormerlySerializedAs("Class")]
        [Tooltip("The old way of assigning a unit's class. This will soon be removed. Use 'UnitClass' instead.")]
        public ModelUnitClass LegacyUnitClass = ModelUnitClass.None;

        /// <summary>
        /// The class/type of this unit
        /// </summary>
        [UnityEngine.Serialization.FormerlySerializedAs("EditorUnitClassRef")]
        [Tooltip("The mandatory class/type of this unit")]
        public EditorUnitClassRef UnitClass = null;

        [Tooltip("The faction that this unit belongs to. Only applicable to some units like ships/stations/cargo")]
        public EditorFaction Faction;

        /// <summary>
        /// A custom name for this unit. If null/empty the game displays the unit name based on other rules.
        /// </summary>
        [Tooltip("A custom name given to the unit. E.g. ship name or bar name")]
        public string Name;

        /// <summary>
        /// Optional custom radius. Only relevant to gas clouds and asteroid clusters
        [Tooltip("Defines the radius of gas clouds and asteroid clusters. Ignored when less than zero")]
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
