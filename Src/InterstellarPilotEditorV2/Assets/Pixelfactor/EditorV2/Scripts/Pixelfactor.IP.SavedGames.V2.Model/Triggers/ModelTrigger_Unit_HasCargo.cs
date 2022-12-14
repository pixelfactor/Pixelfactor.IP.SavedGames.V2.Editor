using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    public class ModelTrigger_Unit_HasCargo : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Unit_HasCargo;

        public ModelCargoClass CargoTypeId { get; set; }

        public ComparisonOp Operator = ComparisonOp.LessThan;

        public int Quantity { get; set; } = 1;

        public ModelUnit TargetUnit { get; set; }
    }
}