using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Missions;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting.Actions
{
    public class EditorAction_MissionCompleteObjective : EditorAction_Base
    {
        /// <summary>
        /// The mission objective that should be completed
        /// </summary>
        public EditorMissionObjective MissionObjective;

        /// <summary>
        /// Whether the objective was completed succesfully
        /// </summary>
        public bool Success = true;
    }
}
