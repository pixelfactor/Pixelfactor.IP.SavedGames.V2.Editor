using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    public class ModelTrigger
    {
        public bool Invert { get; set; } = false;

        public virtual TriggerType Type
        {
            get { return TriggerType.Unspecified; }
        }
    }
}
