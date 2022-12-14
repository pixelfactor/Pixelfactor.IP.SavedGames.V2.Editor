namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    using Pixelfactor.IP.Common.Triggers;

    public class ModelTrigger_Player_NearUnit : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Player_NearSectorTarget;

        public float Distance { get; set; } = 200.0f;
        public ModelUnit TargetUnit { get; set; }
    }
}