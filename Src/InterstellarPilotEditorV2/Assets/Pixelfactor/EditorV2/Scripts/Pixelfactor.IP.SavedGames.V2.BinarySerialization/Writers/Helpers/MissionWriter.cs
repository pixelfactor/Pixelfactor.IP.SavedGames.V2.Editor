using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs.Missions;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers.Helpers
{
    public static class MissionWriter
    {
        public static void Write(BinaryWriter writer, ModelMission mission)
        {
            writer.Write((int)mission.MissionType);
            writer.Write(mission.Id);
            writer.WriteStringOrEmpty(mission.Title);
            writer.Write(mission.IsPrimary);
            writer.Write(mission.NotificationsEnabled);
            writer.Write(mission.IsActive);
            writer.Write(mission.StageIndex);
            writer.Write(mission.IsFinished);
            writer.Write(mission.CompletionSuccess);
            writer.Write(mission.ShowInJournal);
            writer.WriteFactionId(mission.OwnerFaction);
            writer.WriteFactionId(mission.MissionGiverFaction);
            writer.Write(mission.CompletionOpinionChange);
            writer.Write(mission.FailureOpinionChange);
            writer.Write(mission.StartTime);
            writer.Write(mission.RewardCredits);

            writer.Write(mission.Stages.Count);
            foreach (var stage in mission.Stages)
            {
                WriteMissionStage(writer, stage);
            }

            writer.Write(mission.Objectives.Count);
            for (int i = 0; i < mission.Objectives.Count; i++)
            {
                var objective = mission.Objectives[i];
                WriteMissionObjective(writer, objective);
            }

            switch (mission.MissionType)
            {
                case MissionType.Courier:
                    {
                        var courierMission = (ModelCourierMission)mission;
                        writer.WriteUnitId(courierMission.PickupUnit);
                        writer.WriteUnitId(courierMission.DestinationUnit);
                        writer.Write((int)courierMission.CargoItem.CargoClass);
                        writer.Write(courierMission.CargoItem.Quantity);
                        writer.Write(courierMission.HasPlayerPickedUpCargo);
                    }
                    break;
                case MissionType.DestroyGroup:
                    {
                        var destroyUnitsMission = (ModelDestroyUnitsMission)mission;
                        writer.Write(destroyUnitsMission.TargetUnits.Count);
                        foreach (var targetUnit in destroyUnitsMission.TargetUnits)
                        {
                            writer.WriteUnitId(targetUnit);
                        }

                        writer.Write(destroyUnitsMission.HasSetGroupHostileToPlayer);
                        writer.WriteFactionId(destroyUnitsMission.TargetFaction);
                        writer.WriteSectorId(destroyUnitsMission.TargetSector);
                        writer.WriteFleetId(destroyUnitsMission.TargetFleet);
                    }
                    break;
                case MissionType.DeliverShip:
                    {
                        var deliverShipMission = (ModelDeliverShipMission)mission;
                        writer.Write((int)deliverShipMission.UnitClass);
                        writer.WriteUnitId(deliverShipMission.DestinationUnit);
                    }
                    break;
                case MissionType.Breakdown:
                    {
                        var breakdownMission = (ModelBreakdownMission)mission;
                        writer.WriteUnitId(breakdownMission.BaseUnit);
                        writer.WriteUnitId(breakdownMission.BreakdownUnit);
                    }
                    break;
            }
        }

        public static void WriteMissionObjective(BinaryWriter writer, ModelMissionObjective objective)
        {
            writer.Write(objective.Id);
            writer.WriteStringOrEmpty(objective.Title);
            writer.WriteStringOrEmpty(objective.Description);
            writer.Write(objective.IsOptional);
            writer.Write(objective.Order);

            writer.Write(objective.IsActive);
            writer.Write(objective.IsComplete);
            writer.Write(objective.Success);
            writer.Write(objective.ShowInJournal);
        }

        public static void WriteMissionStage(BinaryWriter writer, ModelMissionStage missionStage)
        {
            writer.Write(missionStage.CompletesMission);
            writer.WriteStringOrEmpty(missionStage.JournalEntry);
            writer.Write(missionStage.MissionSuccess);
        }
    }
}
