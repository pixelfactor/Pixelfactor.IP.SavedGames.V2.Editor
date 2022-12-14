using Pixelfactor.IP.SavedGames.V2.Model;
using System.Collections.Generic;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers.Helpers
{
    public static class FleetSpawnParamsReader
    {
        public static ModelFleetSpawnParams Read(
            BinaryReader reader,
            Dictionary<int, ModelFaction> factions,
            Dictionary<int, ModelSector> sectors,
            Dictionary<int, ModelUnit> units)
        {
            var spawnParams = new ModelFleetSpawnParams();
            spawnParams.TargetSector = reader.ReadSector(sectors);
            spawnParams.TargetPosition = reader.ReadVec3();
            spawnParams.TargetDockUnit = reader.ReadUnit(units);
            spawnParams.FleetResourceName = reader.ReadString();
            spawnParams.Faction = reader.ReadFaction(factions);
            spawnParams.ShipDesignation = reader.ReadString();
            spawnParams.HomeSector = reader.ReadSector(sectors);
            spawnParams.HomeBaseUnit = reader.ReadUnit(units);

            // Read ships
            var count = reader.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                var shipParams = ReadFleetSpawnParamsItem(reader);
                spawnParams.Items.Add(shipParams);
            }

            return spawnParams;
        }

        public static ModelFleetSpawnParamsItem ReadFleetSpawnParamsItem(BinaryReader reader)
        {
            var item = new ModelFleetSpawnParamsItem();
            item.UnitClass = (ModelUnitClass)reader.ReadInt32();
            item.PilotResourceName = reader.ReadString();
            item.ShipName = reader.ReadString();
            return item;
        }
    }
}
