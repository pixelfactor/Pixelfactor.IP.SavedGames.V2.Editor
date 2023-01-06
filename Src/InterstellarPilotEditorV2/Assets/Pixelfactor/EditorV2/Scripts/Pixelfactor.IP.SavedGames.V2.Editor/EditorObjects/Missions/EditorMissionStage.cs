using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Missions
{
    /// <summary>
    /// Defines the state of a mission. The mission can only be in one stage at a time. <br />
    /// The purpose of mission stages is to show more useful info in the player's log about the state of the mission.<br />
    /// Most simple missions will require only one stage
    /// </summary>
    public class EditorMissionStage : MonoBehaviour
    {
        [Multiline(6)]
        public string Description = "";

        /// <summary>
        /// Whether this stage automatically completes the mission when activated
        /// </summary>
        public bool CompletesMission = false;

        /// <summary>
        /// When <see cref="CompletesMission"/> is true, determines whether this stage completes or fails the mission
        /// </summary>
        public bool CompletesMissionSuccess = true;
    }
}
