using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Missions;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorPlayer : MonoBehaviour
    {
        public EditorPerson Person;

        /// <summary>
        /// Gets the mission that the player has activated guidance for. Only one mission can have guidance activated at one time<br />
        /// When this is active, the HUD highlights any mission objectives
        /// </summary>
        public EditorMission ActiveMission;
    }
}
