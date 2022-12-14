namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    using Pixelfactor.IP.Common.Triggers;
    using UnityEngine;

    public class ModelTrigger_Scenario_TimeSinceDialogShown : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Scenario_TimeSinceDialogShown;

        public bool IncludePendingRequests { get; set; } = true;
        public float TimeSinceShown { get; set; } = 0.5f;
    }
}