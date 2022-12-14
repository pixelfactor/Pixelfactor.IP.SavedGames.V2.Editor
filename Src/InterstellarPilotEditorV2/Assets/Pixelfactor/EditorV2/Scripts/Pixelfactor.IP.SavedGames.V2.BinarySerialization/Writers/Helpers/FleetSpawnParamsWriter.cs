﻿using Pixelfactor.IP.SavedGames.V2.Model;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers.Helpers
{
    public static class FleetSpawnParamsWriter
    {
        public static void Write(BinaryWriter writer, ModelFleetSpawnParams spawnParams)
        {
            writer.WriteSectorId(spawnParams.TargetSector);
            writer.WriteVec3(spawnParams.TargetPosition);
            writer.WriteUnitId(spawnParams.TargetDockUnit);
            writer.WriteStringOrEmpty(spawnParams.FleetResourceName);
            writer.WriteFactionId(spawnParams.Faction);
            writer.WriteStringOrEmpty(spawnParams.ShipDesignation);
            writer.WriteSectorId(spawnParams.HomeSector);
            writer.WriteUnitId(spawnParams.HomeBaseUnit);

            writer.Write(spawnParams.Items.Count);
            foreach (var item in spawnParams.Items)
            {
                WriteFleetSpawnParamsItem(writer, item);
            }
        }

        private static void WriteFleetSpawnParamsItem(BinaryWriter writer, ModelFleetSpawnParamsItem item)
        {
            writer.Write((int)item.UnitClass);
            writer.WriteStringOrEmpty(item.PilotResourceName);
            writer.WriteStringOrEmpty(item.ShipName);
        }
    }
}
