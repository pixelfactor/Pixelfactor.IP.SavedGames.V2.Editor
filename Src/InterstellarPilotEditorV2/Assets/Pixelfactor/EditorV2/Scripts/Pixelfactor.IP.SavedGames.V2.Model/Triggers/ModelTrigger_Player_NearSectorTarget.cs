namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    using Pixelfactor.IP.Common.Triggers;
    using UnityEngine;

    public class ModelTrigger_Player_NearSectorTarget : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Player_NearSectorTarget;

        public float Distance { get; set; } = 200.0f;
        public ModelSectorTarget Target { get; set; }
    }
}