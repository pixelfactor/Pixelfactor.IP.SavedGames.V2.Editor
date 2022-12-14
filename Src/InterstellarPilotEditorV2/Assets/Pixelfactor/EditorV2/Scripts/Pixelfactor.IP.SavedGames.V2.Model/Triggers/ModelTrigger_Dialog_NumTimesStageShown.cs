namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    using Pixelfactor.IP.Common.Triggers;

    public class ModelTrigger_Dialog_NumTimesStageShown : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Dialog_NumTimesStageShown;

        public int Count { get; set; } = 0;
        public int DialogStageId { get; set; }
    }
}