using Pixelfactor.IP.Common;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Represents factory progress at a station e.g. production of alloys at the refinery
    /// </summary>
    public class ModelComponentUnitFactoryItemData
    {
        public CargoFactoryItemState State { get; set; }
        public float ProductionElapsed { get; set; }
    }
}
