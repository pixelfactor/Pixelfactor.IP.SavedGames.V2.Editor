using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Missions;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Represents the human player
    /// </summary>
    public class EditorPlayer : MonoBehaviour
    {
        [Tooltip("Mandatory person that the player is represented by")]
        public EditorPerson Person;

        /// <summary>
        /// Gets the mission that the player has activated guidance for. Only one mission can have guidance activated at one time<br />
        /// When this is active, the HUD highlights any mission objectives
        /// </summary>
        [Tooltip("Optional mission that has guidance enabled i.e. the HUD will show the location of mission objectives")]
        public EditorMission ActiveMission;
    }
}
