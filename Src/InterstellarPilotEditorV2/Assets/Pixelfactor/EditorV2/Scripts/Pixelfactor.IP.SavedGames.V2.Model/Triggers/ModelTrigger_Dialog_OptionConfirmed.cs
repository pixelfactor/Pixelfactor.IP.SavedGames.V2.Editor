namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    using Pixelfactor.IP.Common.Triggers;

    /// <summary>
    /// Trigger occurs when the player selects a dialog option
    /// </summary>
    public class ModelTrigger_Dialog_OptionConfirmed : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Dialog_OptionConfirmed;

        public int DialogOptionId { get; set; }
    }
}