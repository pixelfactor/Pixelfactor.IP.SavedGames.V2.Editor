using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    /// <summary>
    /// Trigger that fires when it is possible to show dialog to the player
    /// This is true when the player is pilotting and an interval has passed since the last dialog was dismissed
    /// </summary>
    public class ModelTrigger_Scenario_AllowDialog : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Scenario_AllowDialog;
    }
}