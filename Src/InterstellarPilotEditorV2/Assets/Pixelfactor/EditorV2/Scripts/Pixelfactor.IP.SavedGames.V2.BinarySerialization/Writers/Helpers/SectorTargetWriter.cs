﻿using Pixelfactor.IP.SavedGames.V2.Model;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers.Helpers
{
    public static class SectorTargetWriter
    {
        public static void Write(BinaryWriter writer, ModelSectorTarget sectorTarget)
        {
            writer.WriteVec3(sectorTarget.Position);
            writer.WriteSectorId(sectorTarget.Sector);
            writer.WriteUnitId(sectorTarget.TargetUnit);
            writer.WriteFleetId(sectorTarget.TargetFleet);
            writer.Write(sectorTarget.HadValidTarget);
        }
    }
}
