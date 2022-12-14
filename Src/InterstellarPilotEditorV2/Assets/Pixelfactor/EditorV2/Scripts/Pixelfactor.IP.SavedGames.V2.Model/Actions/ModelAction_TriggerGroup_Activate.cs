namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    using Pixelfactor.IP.Common.Triggers;
    using Pixelfactor.IP.SavedGames.V2.Model.Triggers;

    /// <summary>
    /// Action that sets the trigger group game object to be active
    /// 
    /// Note that the TriggerGroup isn't saved
    /// </summary>
    public class ModelAction_TriggerGroup_Activate : ModelAction
    {
        public override ActionType Type => ActionType.TriggerGroup_Activate;

        public ModelTriggerGroup TriggerGroup { get; set; }
    }
}