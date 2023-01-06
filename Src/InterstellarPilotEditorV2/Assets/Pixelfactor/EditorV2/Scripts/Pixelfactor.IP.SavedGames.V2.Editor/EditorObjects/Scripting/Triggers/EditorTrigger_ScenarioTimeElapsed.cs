namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting.Triggers
{
    /// <summary>
    /// Trigger that fires when a certain amount of scenario time has elapsed.
    /// Scenario time is different from real time and speeds up with e.g. hypersleep
    /// </summary>
    public class EditorTrigger_ScenarioTimeElapsed : EditorTrigger_Base
    {
        /// <summary>
        /// The time before evaluation in seconds
        /// </summary>
        public double Time = 2.0;
    }
}
