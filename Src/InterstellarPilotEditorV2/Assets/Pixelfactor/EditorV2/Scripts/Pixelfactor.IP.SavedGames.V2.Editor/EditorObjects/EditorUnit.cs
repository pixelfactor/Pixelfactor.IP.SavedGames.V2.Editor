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

        public EditorUnitClassRef EditorUnitClassRef = null;

        public EditorFaction Faction;
        /// <summary>
        /// Requisition point provision. Provides faction with chance to build more units
        /// </summary>
        public int RpProvision;

        /// <summary>
        /// A custom name for this unit. If null/empty the game displays the unit name based on other rules.
        /// </summary>
        public string Name;

        void OnDrawGizmosSelected()
        {
            if (SelectionHelper.IsSelected(this) || SelectionHelper.IsSelected(this.transform.parent))
            { 
                DrawString.Draw(this.gameObject.name, this.transform.position, Color.white);
            }
        }

        public ModelUnitClass Class
        {
            get
            {
                if (this.EditorUnitClassRef != null)
                {
                    return this.EditorUnitClassRef.UnitClass;
                }

                return ModelUnitClass.None;
            }
        }
    }
}
