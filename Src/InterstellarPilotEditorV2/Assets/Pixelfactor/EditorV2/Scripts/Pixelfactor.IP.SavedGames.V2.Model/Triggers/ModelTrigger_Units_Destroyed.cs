namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    using Pixelfactor.IP.Common.Triggers;
    using System.Collections.Generic;

    public class ModelTrigger_Units_Destroyed : ModelTrigger
    {
        public override TriggerType Type => TriggerType.Units_Destroyed;

        /// <summary>
        /// The minimum amount of units that can be destroyed to fire the trigger. Use -1 to disregard and require ALL to be destroyed
        /// </summary>
        public int MinCount { get; set; } = -1;

        public List<ModelUnit> Units = new List<ModelUnit>();
    }
}