using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Missions
{
    /// <summary>
    /// A mission defines a task that the player needs to complete.
    /// Missions can have one or more stages and or more objectives
    /// </summary>
    public class EditorMission : MonoBehaviour
    {
        public int UniqueId = -1;

        /// <summary>
        /// Title is displayed in the player's log
        /// </summary>
        public string Title = "New mission";

        /// <summary>
        /// Optional faction that is the source of the mission
        /// </summary>
        public EditorFaction MissionGiver;

        /// <summary>
        /// Change in faction relation when the mission is completed
        /// </summary>
        [Range(-1.0f, 1.0f)]
        public float CompletionOpinionChange = 0.0f;

        /// <summary>
        /// Change in faction relation when the mission is failed
        /// </summary>
        [Range(-1.0f, 1.0f)]
        public float FailureOpinionChange = 0.0f;

        /// <summary>
        /// Optional credits givenn to the player when completing the mission
        /// </summary>
        public int MissionRewardCredits = 0;

        /// <summary>
        /// Whether the show notifications to the player when the mission state changes (e.g. "Mission complete" message)
        /// </summary>
        public bool BroadcastMessages = true;

        /// <summary>
        /// When true, completion or failure of this mission will automatically complete or fail the scenario
        /// </summary>
        public bool IsPrimaryMission = false;

        /// <summary>
        /// Whether to allow the player to view the mission in the mission log. Generally this should be true.<br />
        /// Turning this off can allow for a hidden/secret mission
        /// </summary>
        public bool ShowInLog = true;
    }
}
