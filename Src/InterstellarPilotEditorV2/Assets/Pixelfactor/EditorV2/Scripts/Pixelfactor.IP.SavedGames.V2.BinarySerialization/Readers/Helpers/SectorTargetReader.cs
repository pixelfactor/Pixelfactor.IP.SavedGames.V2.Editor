using Pixelfactor.IP.SavedGames.V2.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers.Helpers
{
    public static class SectorTargetReader
    {
        public static ModelSectorTarget Read(
            BinaryReader reader, 
            Dictionary<int, ModelSector> sectors,
            Dictionary<int, ModelUnit> units,
            Dictionary<int, ModelFleet> fleets)
        {
            var sectorTarget = new ModelSectorTarget();
            sectorTarget.Position = reader.ReadVec3();

            var sectorId = reader.ReadInt32();
            var targetUnitId = reader.ReadInt32();
            var targetFleetId = reader.ReadInt32();

            sectorTarget.Sector = sectors.GetValueOrDefault(sectorId);
            sectorTarget.TargetUnit = units.GetValueOrDefault(targetUnitId);
            sectorTarget.TargetFleet = fleets.GetValueOrDefault(targetFleetId);
            sectorTarget.HadValidTarget = reader.ReadBoolean();
            return sectorTarget;
        }
    }
}
