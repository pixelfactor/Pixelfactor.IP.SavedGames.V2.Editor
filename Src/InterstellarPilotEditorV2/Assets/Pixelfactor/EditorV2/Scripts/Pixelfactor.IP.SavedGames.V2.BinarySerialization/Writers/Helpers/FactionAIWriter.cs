using Pixelfactor.IP.Common.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.Factions.FactionAITypes;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers.Helpers
{
    public static class FactionAIWriter
    {
        public static void Write(BinaryWriter writer, FactionAIType factionAIType, ModelFactionAI factionAI)
        {
            writer.Write(factionAI.NextUnitSpawnTime);
            writer.Write(factionAI.NumFleetsSpawned);
            writer.Write(factionAI.NumUnitsSpawned);
            writer.Write(factionAI.SpawnOnlyAtOwnedDocks);
            writer.Write(factionAI.LastBuiltUnitTime);
            writer.Write(factionAI.LastOrderedPatrolTime);

            writer.Write((int)factionAI.SpawnMode);
            writer.Write(factionAI.SpawnSectors.Count);
            foreach (var sector in factionAI.SpawnSectors)
            {
                writer.WriteSectorId(sector);
            }

            switch (factionAIType)
            {
                case FactionAIType.Miner:
                case FactionAIType.Patroller:
                case FactionAIType.Mercenary:
                    {
                        // Nothing to read
                    }
                    break;
                case FactionAIType.Trader:
                    {
                        var trader = (ModelFactionAITrader)factionAI;
                        writer.Write(trader.TradeOnlySpecificCargoTypes);

                        writer.Write(trader.TradeSpecificCargoTypes.Count);
                        foreach (var cargoClass in trader.TradeSpecificCargoTypes)
                        {
                            writer.Write((int)cargoClass);
                        }
                    }
                    break;
            }
        }
    }
}
