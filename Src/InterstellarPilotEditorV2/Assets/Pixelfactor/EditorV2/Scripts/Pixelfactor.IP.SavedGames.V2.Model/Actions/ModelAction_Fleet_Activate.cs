namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    using Pixelfactor.IP.Common.Triggers;

    /// <summary>
    /// Action that sets the trigger group game object to be active
    /// </summary>
    public class ModelAction_Fleet_Activate : ModelAction
    {
        public override ActionType Type => ActionType.Fleet_Activate;
    }
}