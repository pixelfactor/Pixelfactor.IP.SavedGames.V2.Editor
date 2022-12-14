using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Docked ships
    /// </summary>
    public class ModelComponentUnitDockData
    {
        public List<ModelComponentUnitDockDataItem> Items { get; set; } = new List<ModelComponentUnitDockDataItem>();
    }
}
