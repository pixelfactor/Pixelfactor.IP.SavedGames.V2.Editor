using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    public class ModelAction_Player_NewMessageSimple : ModelAction
    {
        public override ActionType Type => ActionType.Player_NewMessageSimple;

        public bool Notifications { get; set; } = true;
        public string From { get; set; } = null;
        public string Message { get; set; } = null;
        public string Subject { get; set; } = null;
        public string To { get; set; } = "#player#";
    }
}
