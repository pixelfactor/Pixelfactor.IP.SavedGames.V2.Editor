using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Bounty
{
    /// <summary>
    /// Represents an item on a bounty board. Nest this object underneath a faction for the editor find it when exporting
    /// </summary>
    public class EditorBountyBoardItem : MonoBehaviour
    {
        /// <summary>
        /// The person that is wanted, the target of the bounty
        /// </summary>
        [Tooltip("The person that is wanted, the target of the bounty")]
        public EditorPerson TargetPerson;

        /// <summary>
        /// Optional game time in seconds when the target of this bounty was last seen
        /// </summary>
        [Tooltip("Optional game time in seconds when the target of this bounty was last seen")]
        public double TimeOfLastSighting = -1;

        /// <summary>
        /// Reward in credits that is granted when the bounty is killed
        /// </summary>
        [Tooltip("Reward in credits that is granted when the bounty is killed")]
        public int Reward = 1000;

        /// <summary>
        /// The faction that placed the bounty
        /// </summary>
        [Tooltip("Optional faction that placed the bounty. If this is left blank, the source faction will automatically be set to the bounty board owner")]
        public EditorFaction SourceFaction = null;

        /// <summary>
        /// Whether to update the last known position of the bounty when exporting
        /// </summary>
        [Header("Advanced")]
        [Tooltip("Optional location where the bounty target was last seen. You can use this to give a 'clue' as to where the bounty was last seen without revealing the action location")]
        public Transform LastKnownLocation = null;

        [Tooltip("Whether to update the last known position of the bounty when exporting if LastKnownPosition has not been set")]
        public bool ExportLastKnownPosition = true;

        /// <summary>
        /// Whether to update the last known position of the bounty when exporting
        /// </summary>
        [Tooltip("Whether to update the last known ship of the bounty target when exporting")]
        public bool ExportLastPilottedUnit = true;
    }
}
