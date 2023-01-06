using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Missions
{
    public class EditorMissionObjective : MonoBehaviour
    {
        /// <summary>
        /// Whether an id is given, this should be globally unique, not just to the parent mission.
        /// </summary>
        public int Id = -1;

        /// <summary>
        /// Title of the mission that will be displayed in mission log
        /// </summary>
        public string Title = "";

        /// <summary>
        /// Description of the mission that will be displayed in the mission log
        /// </summary>
        [Multiline(6)]
        public string Description = "";

        /// <summary>
        /// Whether the mission has been given to the player<br />
        /// This can be turned off if objectives aren't relevant until some other event occurs.
        /// </summary>
        public bool IsActive = true;

        /// <summary>
        /// Whether the player can choose to disregard this objective
        /// </summary>
        public bool IsOptional = false;

        /// <summary>
        /// Whether the objective is already complete
        /// </summary>
        public bool IsComplete = false;

        /// <summary>
        /// When <see cref="IsComplete"/> is true, whether the objective was completed successfully
        /// </summary>
        public bool WasSuccessful = false;
    }
}
