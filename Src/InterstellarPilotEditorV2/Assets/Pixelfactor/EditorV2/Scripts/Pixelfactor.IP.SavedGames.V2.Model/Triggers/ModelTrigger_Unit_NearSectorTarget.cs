namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    using Pixelfactor.IP.Common.Triggers;
    using UnityEngine;

    public class ModelTrigger_Unit_NearSectorTarget : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Unit_NearSectorTarget;

        public float Distance { get; set; } = 200.0f;
        public ModelUnit TargetUnit { get; set; }

        public ModelSectorTarget SectorTarget { get; set; }
    }
}