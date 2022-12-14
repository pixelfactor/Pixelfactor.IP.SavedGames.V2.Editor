using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    /// <summary>
    /// Sets the Hud target to a specific unit
    /// </summary>
    public class ModelAction_Hud_SetTarget : ModelAction
    {
        public override ActionType Type => ActionType.Hud_SetTarget;
    }
}