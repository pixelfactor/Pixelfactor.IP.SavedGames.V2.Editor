using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs.JobTypes;
using System.Collections.Generic;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers.Helpers
{
    public static class JobReader
    {
        public static ModelJob Read(
            BinaryReader reader,
            JobType jobType,
            Dictionary<int, ModelSector> sectors,
            Dictionary<int, ModelFaction> factions,
            Dictionary<int, ModelUnit> units)
        {
            var job = CreateJobFromJobType.CreateJob(jobType);
            job.Id = reader.ReadInt32();

            // TODO: Validate
            var unitId = reader.ReadInt32();
            job.Unit = units.GetValueOrDefault(unitId);

            // TODO: Validate
            var factionId = reader.ReadInt32();
            job.Faction = factions.GetValueOrDefault(factionId);

            job.ExpiryTime = reader.ReadDouble();
            job.RewardCredits = reader.ReadInt32();
            job.ProfitCredits = reader.ReadInt32();

            switch (job.JobType)
            {
                case JobType.Courier:
                    {
                        var m = (ModelCourierJob)job;

                        // TODO: Validate
                        var pickupUnitId = reader.ReadInt32();
                        m.PickupUnit = units.GetValueOrDefault(pickupUnitId);

                        var destinationUnitId = reader.ReadInt32();
                        m.DestinationUnit = units.GetValueOrDefault(destinationUnitId);

                        m.Cargo = ComponentUnitCargoDataItemReader.Read(reader);
                    }
                    break;
                case JobType.DeliverShip:
                    {
                        var m = (ModelDeliverShipJob)job;

                        // TODO: Validate
                        m.UnitClass = (ModelUnitClass)reader.ReadInt32();

                        var destinationUnitId = reader.ReadInt32();
                        m.DestinationUnit = units.GetValueOrDefault(destinationUnitId);
                    }
                    break;
                case JobType.Breakdown:
                    {
                        var m = (ModelBreakdownJob)job;
                        m.BreakdownUnitClass = (ModelUnitClass)reader.ReadInt32();

                        var sectorId = reader.ReadInt32();
                        m.BreakdownDestinationSector = sectors.GetValueOrDefault(sectorId);
                        m.BreakdownDestinationPosition = reader.ReadVec3();
                    }
                    break;
                case JobType.DestroyGroup:
                    {
                        var m = (ModelDestroyFleetJob)job;
                        m.FleetSpawnParams = FleetSpawnParamsReader.Read(reader, factions, sectors, units);
                    }
                    break;
            }

            return job;
        }
    }
}
