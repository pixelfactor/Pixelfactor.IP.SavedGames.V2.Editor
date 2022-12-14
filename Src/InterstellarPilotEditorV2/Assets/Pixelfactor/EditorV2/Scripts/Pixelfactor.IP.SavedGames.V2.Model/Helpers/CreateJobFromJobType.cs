using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs.JobTypes;
using System;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public static class CreateJobFromJobType
    {
        public static ModelJob CreateJob(JobType jobType)
        {
            switch (jobType)
            {
                case JobType.DestroyGroup:
                    return new ModelDestroyFleetJob();
                case JobType.Courier:
                    return new ModelCourierJob();
                case JobType.DeliverShip:
                    return new ModelDeliverShipJob();
                case JobType.Breakdown:
                    return new ModelBreakdownJob();
                default:
                    throw new NotImplementedException($"Unknown job type {(int)jobType}");
            }
        }
    }
}
