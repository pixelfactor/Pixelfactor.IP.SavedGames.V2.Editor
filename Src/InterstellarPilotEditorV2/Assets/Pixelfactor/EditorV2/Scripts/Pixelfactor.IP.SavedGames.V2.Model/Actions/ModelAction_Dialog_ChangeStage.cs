namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    using Pixelfactor.IP.Common.Triggers;

    /// <summary>
    /// Changes the current dialog stage. Use this as a way of changing, initialising or ending a conversation
    /// Use a Null Stage to end any current dialog
    /// </summary>
    public class ModelAction_Dialog_ChangeStage : ModelAction
    {
        public override ActionType Type => ActionType.Dialog_ChangeStage;
    }
}