using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    /// <summary>
    /// Trigger that fires when the hud target is a specific unit
    /// </summary>
    public class ModelTrigger_Player_CurrentHudTarget : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Player_CurrentHudTarget;

        public ModelUnit TargetUnit { get; set; }
    }
}