using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Defines info about ships that a station sells
    /// </summary>
    public class ModelUnitShipTraderData
    {
        public List<ModelUnitShipTraderItem> Items { get; set; } = new List<ModelUnitShipTraderItem>();
    }
}
