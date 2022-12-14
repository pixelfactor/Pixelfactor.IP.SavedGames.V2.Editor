using Pixelfactor.IP.Common.Factions;
using Pixelfactor.IP.SavedGames.V2.Model;
using Pixelfactor.IP.SavedGames.V2.Model.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.Factions.FactionAITypes;
using System.Collections.Generic;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers.Helpers
{
    public static class FactionAIReader
    {
        public static ModelFactionAI Read(BinaryReader reader, FactionAIType factionAIType, Dictionary<int, ModelSector> sectors)
        {
            var factionAI = CreateFactionAIFromType.Create(factionAIType);
            factionAI.NextUnitSpawnTime = reader.ReadDouble();
            factionAI.NumFleetsSpawned = reader.ReadInt32();
            factionAI.NumUnitsSpawned = reader.ReadInt32();
            factionAI.SpawnOnlyAtOwnedDocks = reader.ReadBoolean();
            factionAI.LastBuiltUnitTime = reader.ReadDouble();
            factionAI.LastOrderedPatrolTime = reader.ReadDouble();

            factionAI.SpawnMode = (FactionSpawnMode)reader.ReadInt32();
            var spawnSceneCount = reader.ReadInt32();
            for (var i = 0; i < spawnSceneCount; i++)
            {
                var sector = reader.ReadSector(sectors);
                if (sector != null)
                {
                    factionAI.SpawnSectors.Add(sector);
                }
            }

            switch (factionAIType
                )
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
                        trader.TradeOnlySpecificCargoTypes = reader.ReadBoolean();

                        var cargoCount = reader.ReadInt32();
                        for (var i = 0; i < cargoCount; i++)
                        {
                            var cargoClassId = reader.ReadInt32();
                            trader.TradeSpecificCargoTypes.Add((ModelCargoClass)cargoClassId);
                        }
                    }
                    break;
            }

            return factionAI;
        }
    }
}
