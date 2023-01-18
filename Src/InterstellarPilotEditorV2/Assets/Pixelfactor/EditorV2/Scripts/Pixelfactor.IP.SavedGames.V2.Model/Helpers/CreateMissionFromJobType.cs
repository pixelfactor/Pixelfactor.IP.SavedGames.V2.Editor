using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs.Missions;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public static class CreateMissionFromJobType
    {
        public static ModelMission Create(JobType jobType)
        {
            switch (jobType)
            {
                case JobType.DeliverShip:
                    return new ModelDeliverShipMission();
                case JobType.Courier:
                    return new ModelCourierMission();
                case JobType.DestroyGroup:
                    return new ModelDestroyUnitsMission();
                case JobType.Breakdown:
                    return new ModelBreakdownMission();
                default:
                    return new ModelMission();
            }
        }
    }

    public static class CreateMissionFromMissionType
    {
        public static ModelMission Create(MissionType jobType)
        {
            switch (jobType)
            {
                case MissionType.DeliverShip:
                    return new ModelDeliverShipMission();
                case MissionType.Courier:
                    return new ModelCourierMission();
                case MissionType.DestroyGroup:
                    return new ModelDestroyUnitsMission();
                case MissionType.Breakdown:
                    return new ModelBreakdownMission();
                default:
                    return new ModelMission();
            }
        }
    }
}
