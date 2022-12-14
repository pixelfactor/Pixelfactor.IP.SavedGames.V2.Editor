using Pixelfactor.IP.Common;
using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    public class ModelTrigger_Fleet_InState : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Fleet_InState;

        public ModelFleet Fleet { get; set; }

        public FleetState State = FleetState.Idle;
    }
}