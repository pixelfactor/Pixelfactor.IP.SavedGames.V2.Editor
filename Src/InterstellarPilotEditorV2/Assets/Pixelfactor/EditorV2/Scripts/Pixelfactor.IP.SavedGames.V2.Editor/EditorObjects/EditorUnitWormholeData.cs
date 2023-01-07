using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorUnitWormholeData : MonoBehaviour
    {
        /// <summary>
        /// For stable wormholes, points to the other wormhole that this connects to.
        /// </summary>
        public EditorUnit TargetWormholeUnit;

        /// <summary>
        /// Determines whether the wormhole is an unstable wormhole - this is where the target of the wormhole will change during the game
        /// </summary>
        public bool IsUnstable = false;

        /// <summary>
        /// Scenario time (in seconds) when the unstable wormhole will change target. You can leave this at zero and the target will change randomly when the scenario is started.<br />
        /// This value can be set extremely high to create a wormhole that always points to a certain game location
        /// </summary>
        public double UnstableNextChangeTargetTime;

        /// <summary>
        /// The target of the wormhole when it is unstable<br />
        /// The target should be a child of a sector<br />
        /// The target should be oriented for the direction the ship should face when exiting the wormhole
        /// </summary>
        public Transform UnstableTarget;

        void OnDrawGizmosSelected()
        {
            if (this.TargetWormholeUnit != null)
            {
                // Draws a blue line from this transform to the target
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, this.TargetWormholeUnit.transform.position);
            }
        }

        public EditorSector GetActualTargetSector()
        {
            if (this.TargetWormholeUnit != null)
            {
                return this.TargetWormholeUnit.GetComponentInParent<EditorSector>();
            }

            return this.UnstableTarget?.GetComponentInParent<EditorSector>();
        }
    }
}
