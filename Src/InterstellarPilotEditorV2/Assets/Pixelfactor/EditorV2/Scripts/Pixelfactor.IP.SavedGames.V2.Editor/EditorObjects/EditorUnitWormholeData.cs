using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines a wormhole where a ship can get from one sector to another
    /// </summary>
    public class EditorUnitWormholeData : MonoBehaviour
    {
        /// <summary>
        /// For stable wormholes, points to the other wormhole that this connects to.
        /// </summary>
        [Tooltip("For stable wormholes, points to the other wormhole that this connects to.")]
        public EditorUnit TargetWormholeUnit;

        /// <summary>
        /// Determines whether the wormhole is an unstable wormhole - this is where the target of the wormhole will change during the game
        /// </summary>
        [Tooltip("Determines whether the wormhole is an unstable wormhole - this is where the target of the wormhole will change during the game")]
        public bool IsUnstable = false;

        /// <summary>
        /// Scenario time (in seconds) when the unstable wormhole will change target. You can leave this at zero and the target will change randomly when the scenario is started.<br />
        /// This value can be set extremely high to create a wormhole that always points to a certain game location
        /// </summary>
        [Tooltip("Scenario time (in seconds) when the unstable wormhole will change target. You can leave this at zero and the target will change randomly when the scenario is started. This value can be set extremely high to create a wormhole that always points to a certain game location")]
        public double UnstableNextChangeTargetTime;

        /// <summary>
        /// The target of the wormhole when it is unstable<br />
        /// The target should be a child of a sector<br />
        /// The target should be oriented for the direction the ship should face when exiting the wormhole
        /// </summary>
        [Tooltip("The target of the wormhole when it is unstable. The target should be a child of a sector. The target should be oriented for the direction the ship should face when exiting the wormhole")]
        public Transform UnstableTarget;

        void OnDrawGizmos()
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

            if (this.UnstableTarget != null)
            { 
                return this.UnstableTarget.GetComponentInParent<EditorSector>();
            }

            return null;
        }
    }
}
