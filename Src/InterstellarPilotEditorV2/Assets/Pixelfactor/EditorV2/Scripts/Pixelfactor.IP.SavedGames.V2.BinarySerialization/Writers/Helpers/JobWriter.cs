using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs.JobTypes;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers.Helpers
{
    public static class JobWriter
    {
        public static void Write(BinaryWriter writer, ModelJob job)
        {
            writer.Write(job.Id);
            writer.WriteUnitId(job.Unit);
            writer.WriteFactionId(job.Faction);
            writer.Write(job.ExpiryTime);
            writer.Write(job.RewardCredits);
            writer.Write(job.ProfitCredits);

            switch (job.JobType)
            {
                case JobType.Courier:
                    {
                        var m = (ModelCourierJob)job;
                        writer.WriteUnitId(m.PickupUnit);
                        writer.WriteUnitId(m.DestinationUnit);
                        ComponentUnitCargoDataItemWriter.Write(writer, m.Cargo);
                    }
                    break;
                case JobType.DeliverShip:
                    {
                        var m = (ModelDeliverShipJob)job;
                        writer.Write((int)m.UnitClass);
                        writer.WriteUnitId(m.DestinationUnit);
                    }
                    break;
                case JobType.Breakdown:
                    {
                        var m = (ModelBreakdownJob)job;
                        writer.Write((int)m.BreakdownUnitClass);
                        writer.WriteSectorId(m.BreakdownDestinationSector);
                        writer.WriteVec3(m.BreakdownDestinationPosition);
                    }
                    break;
                case JobType.DestroyGroup:
                    {
                        var m = (ModelDestroyFleetJob)job;
                        FleetSpawnParamsWriter.Write(writer, m.FleetSpawnParams);
                    }
                    break;
            }
        }
    }
}
