﻿namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    using Pixelfactor.IP.Common.Triggers;
    using Pixelfactor.IP.SavedGames.V2.Model.Jobs;

    public class ModelAction_Mission_CompleteObjective : ModelAction
    {
        public override ActionType Type => ActionType.Mission_CompleteObjective;

        public ModelMissionObjective MissionObjective { get; set; }

        public bool Success { get; set; } = true;
    }
}