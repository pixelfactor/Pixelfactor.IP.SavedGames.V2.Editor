namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    using Pixelfactor.IP.Common.Triggers;

    /// <summary>
    /// Only use this class for targets that are persistent
    /// </summary>
    public class ModelAction_Misc_ActivateGameObject : ModelAction
    {
        public override ActionType Type => ActionType.Misc_ActivateGameObject;
    }
}