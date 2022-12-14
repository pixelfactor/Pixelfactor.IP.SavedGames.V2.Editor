using Pixelfactor.IP.SavedGames.V2.Model;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers.Helpers
{
    public static class CustomTradeRouteReader
    {
        public static ModelCustomTradeRoute Read(BinaryReader reader, Dictionary<int, ModelUnit> units)
        {
            var tradeRoute = new ModelCustomTradeRoute();
            var cargoClassId = reader.ReadInt32();
            var buyLocationUnitId = reader.ReadInt32();
            var sellLocationUnitId = reader.ReadInt32();
            var buyPriceMultiplier = reader.ReadSingle();

            // TODO: Verify all this stuff
            tradeRoute.CargoClass = (ModelCargoClass)cargoClassId;
            tradeRoute.BuyLocation = units.GetValueOrDefault(buyLocationUnitId);
            tradeRoute.SellLocation = units.GetValueOrDefault(sellLocationUnitId);
            tradeRoute.BuyPriceMultiplier = buyPriceMultiplier;

            return tradeRoute;
        }
    }
}
