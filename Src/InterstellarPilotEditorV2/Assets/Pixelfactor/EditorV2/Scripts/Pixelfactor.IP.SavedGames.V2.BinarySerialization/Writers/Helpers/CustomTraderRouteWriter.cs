﻿using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.Models;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers.Helpers
{
    public static class CustomTraderRouteWriter
    {
        public static void Write(BinaryWriter writer, ModelCustomTradeRoute customTradeRoute)
        {
            writer.Write((int)customTradeRoute.CargoClass);
            writer.WriteUnitId(customTradeRoute.BuyLocation);
            writer.WriteUnitId(customTradeRoute.SellLocation);
            writer.Write(customTradeRoute.BuyPriceMultiplier);
        }
    }
}
