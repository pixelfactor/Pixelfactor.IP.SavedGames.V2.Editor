using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    /// <summary>
    /// Not to be confused with the DialogController which is more interactive and allows player options
    /// This will show a simple message on the HUD
    /// </summary>
    public class ModelAction_Hud_ShowSpeech : ModelAction
    {
        public override ActionType Type => ActionType.Hud_ShowSpeech;
    }
}