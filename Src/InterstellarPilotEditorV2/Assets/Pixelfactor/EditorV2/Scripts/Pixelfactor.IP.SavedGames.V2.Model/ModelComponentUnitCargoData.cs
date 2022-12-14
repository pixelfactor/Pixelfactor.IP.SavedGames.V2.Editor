using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// The cargo this ship/station is carrying
    /// </summary>
    public class ModelComponentUnitCargoData
    {
        public List<ModelComponentUnitCargoDataItem> Items { get; set; } = new List<ModelComponentUnitCargoDataItem>();
    }
}
