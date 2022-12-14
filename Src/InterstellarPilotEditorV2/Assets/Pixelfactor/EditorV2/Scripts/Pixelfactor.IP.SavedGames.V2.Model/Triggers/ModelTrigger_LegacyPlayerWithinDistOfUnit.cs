namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    using Pixelfactor.IP.Common.Triggers;

    public class ModelTrigger_LegacyPlayerWithinDistOfUnit : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Player_NearUnit;

        public float Distance { get; set; } = 200.0f;
        public ModelUnit TargetUnit { get; set; }
    }
}