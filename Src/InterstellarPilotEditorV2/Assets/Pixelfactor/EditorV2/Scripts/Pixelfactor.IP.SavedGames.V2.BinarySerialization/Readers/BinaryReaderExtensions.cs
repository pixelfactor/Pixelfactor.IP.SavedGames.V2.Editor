using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using Pixelfactor.IP.SavedGames.V2.Model.Triggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers
{
    public static class BinaryReaderExtensions
    {
        public static Vec3 ReadVec3(this BinaryReader reader)
        {
            var v = new Vec3();
            v.X = reader.ReadSingle();
            v.Y = reader.ReadSingle();
            v.Z = reader.ReadSingle();
            return v;
        }

        public static Vec4 ReadVec4(this BinaryReader reader)
        {
            var v = new Vec4();
            v.X = reader.ReadSingle();
            v.Y = reader.ReadSingle();
            v.Z = reader.ReadSingle();
            v.W = reader.ReadSingle();
            return v;
        }

        public static Vec3? ReadNullableVec3(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadVec3();
            }

            return null;
        }

        public static ModelDamageType ReadDamageType(this BinaryReader reader)
        {
            var damageType = new ModelDamageType();
            damageType.Damage = reader.ReadSingle();
            damageType.MiningDamage = reader.ReadSingle();
            damageType.ShieldDamageType = (ShieldDamageType)reader.ReadInt32();
            return damageType;
        }

        public static ModelMission ReadMission(this BinaryReader reader, Dictionary<int, ModelMission> missionsById)
        {
            var missionId = reader.ReadInt32();
            return missionsById.GetValueOrDefault(missionId);
        }

        public static ModelMissionObjective ReadMissionObjective(this BinaryReader reader, Dictionary<int, ModelMissionObjective> missionObjectivesById)
        {
            var missionObjectiveId = reader.ReadInt32();
            return missionObjectivesById.GetValueOrDefault(missionObjectiveId);
        }

        public static ModelTriggerGroup ReadTriggerGroup(this BinaryReader reader, Dictionary<int, ModelTriggerGroup> triggersGroupsById)
        {
            var triggerGroupId = reader.ReadInt32();
            return triggersGroupsById.GetValueOrDefault(triggerGroupId);
        }

        public static ModelUnit ReadUnit(this BinaryReader reader, Dictionary<int, ModelUnit> unitsById)
        {
            var unitId = reader.ReadInt32();
            return unitsById.GetValueOrDefault(unitId);
        }

        public static ModelFaction ReadFaction(this BinaryReader reader, Dictionary<int, ModelFaction> factions)
        {
            var factionId = reader.ReadInt32();
            return factions.GetValueOrDefault(factionId);
        }

        public static ModelSector ReadSector(this BinaryReader reader, Dictionary<int, ModelSector> sectors)
        {
            var sectorId = reader.ReadInt32();
            return sectors.GetValueOrDefault(sectorId);
        }

        public static ModelPerson ReadPerson(this BinaryReader reader, Dictionary<int, ModelPerson> people)
        {
            var personId = reader.ReadInt32();
            return people.GetValueOrDefault(personId);
        }

        public static ModelFleet ReadFleet(this BinaryReader reader, Dictionary<int, ModelFleet> fleets)
        {
            var fleetId = reader.ReadInt32();
            return fleets.GetValueOrDefault(fleetId);
        }

        public static Version ReadVersion(this BinaryReader reader)
        {
            var versionMajor = reader.ReadInt32();
            var versionMinor = reader.ReadInt32();
            var versionBuild = reader.ReadInt32();
            return new Version(versionMajor, versionMinor, versionBuild);
        }
    }
}
