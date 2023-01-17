using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Missions;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public static class Validator
    {
        public static void Validate(EditorScenario editorScenario, bool throwOnError)
        {
            var settings = CustomSettings.GetOrCreateSettings();

            if (editorScenario.GetSectors().Length == 0)
            {
                OnError("A scenario must have at least one sector", editorScenario, throwOnError);

            }
            // Only validate missing ids if they aren't set on export
            if (!settings.Export_AutosetIds)
            { 
                ValidateMissingIds(editorScenario, throwOnError);
            }

            ValidateAsteroidClusterTypes(editorScenario);

            ValidateDockedUnits(editorScenario, throwOnError);

            ValidateDuplicateIds(editorScenario, throwOnError);

            ValidateDuplicatePilotNames(editorScenario);
            ValidateDuplicateShipNames(editorScenario);

            ValidateUnits(editorScenario, throwOnError);

            Debug.Log("Validation complete");
        }

        private static void ValidateAsteroidClusterTypes(EditorScenario editorScenario)
        {
            foreach (var sector in editorScenario.GetSectors())
            {
                var asteroidClusters = sector.GetComponentsInChildren<EditorAsteroidCluster>();
                if (asteroidClusters.Length > 0 && asteroidClusters.GroupBy(e => e.AsteroidType).Count() > 1)
                {
                    Debug.LogWarning($"Sector {sector.Name} contains asteroid clusters of different types. Was this intended?", sector);
                }
            }
        }

        public static void ValidateDockedUnits(EditorScenario editorScenario, bool throwOnError)
        {
        }

        public static void ValidateDuplicateShipNames(EditorScenario editorScenario)
        {
            var shipGroups = editorScenario.GetComponentsInChildren<EditorComponentUnitData>()
                .Where(e =>
                {
                    var unit = e.GetComponentInParent<EditorUnit>();
                    return unit.ModelUnitClass.IsShipOrStation() && !unit.ModelUnitClass.IsTurret() && !string.IsNullOrWhiteSpace(unit.Name);
                }).GroupBy(e => e.GetComponentInParent<EditorUnit>().Name).Where(e => e.Count() > 1);
            foreach (var shipGroup in shipGroups)
            {
                foreach (var ship in shipGroup)
                {
                    Debug.LogWarning($"Duplicate ship name detected: {ship.GetComponentInParent<EditorUnit>().Name}", ship);
                }
            }
        }

        public static void ValidateDuplicatePilotNames(EditorScenario editorScenario)
        {
            var people = editorScenario.GetComponentsInChildren<EditorPerson>()
                .Where(e => !string.IsNullOrWhiteSpace(e.CustomName)).GroupBy(e => e.CustomName).Where(e => e.Count() > 1);
            foreach (var personGroup in people)
            {
                foreach (var person in personGroup)
                {
                    Debug.LogWarning("Duplicate person name detected", person);
                }
            }
        }

        private static void ValidateMissingIds(EditorScenario editorScenario, bool throwOnError)
        {
            ValidateMessageIds(editorScenario, throwOnError);
            ValidateUnitIds(editorScenario, throwOnError);
            ValidateFactionIds(editorScenario, throwOnError);
            ValidatePersonIds(editorScenario, throwOnError);
            ValidateSectorIds(editorScenario, throwOnError);
            ValidateFleetIds(editorScenario, throwOnError);
            ValidateMissionIds(editorScenario, throwOnError);
            ValidateTriggerGroupIds(editorScenario, throwOnError);
            ValidateMissionObjectiveIds(editorScenario, throwOnError);
        }

        private static void ValidateDuplicateIds(EditorScenario editorScenario, bool throwOnError)
        {
            ValidateDuplicateSectors(editorScenario, throwOnError);
            ValidateDuplicateFactions(editorScenario, throwOnError);
            ValidateDuplicatePeople(editorScenario, throwOnError);
            ValidateDuplicateUnits(editorScenario, throwOnError);
            ValidateDuplicateMessageIds(editorScenario, throwOnError);
            ValidateDuplicateMissionIds(editorScenario, throwOnError);
            ValidateDuplicateTriggerGroupIds(editorScenario, throwOnError);
            ValidateDuplicateMissionObjectiveIds(editorScenario, throwOnError);
        }

        private static void ValidateMessageIds(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var message in editorScenario.GetComponentsInChildren<EditorPlayerMessage>())
            {
                if (message.Id < 0)
                {
                    OnError("All messages require a valid (>0) id", message, throwOnError);
                }
            }
        }

        private static void ValidateTriggerGroupIds(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var triggerGroup in editorScenario.GetComponentsInChildren<EditorTriggerGroup>())
            {
                if (triggerGroup.Id < 0)
                {
                    OnError("All trigger groups require a valid (>0) id", triggerGroup, throwOnError);
                }
            }
        }

        private static void ValidateMissionIds(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var mission in editorScenario.GetComponentsInChildren<EditorMission>())
            {
                if (mission.Id < 0)
                {
                    OnError("All missions require a valid (>0) id", mission, throwOnError);
                }
            }

        }
        private static void ValidateMissionObjectiveIds(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var missionObjective in editorScenario.GetComponentsInChildren<EditorMissionObjective>())
            {
                if (missionObjective.Id < 0)
                {
                    OnError("All mission objectives require a valid (>0) id", missionObjective, throwOnError);
                }
            }
        }

        private static void ValidateFleetIds(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var fleet in editorScenario.GetComponentsInChildren<EditorFleet>())
            {
                if (fleet.Id < 0)
                {
                    OnError("All fleets require a valid (>0) id", fleet, throwOnError);
                }
            }
        }

        private static void ValidateUnits(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var unit in editorScenario.GetComponentsInChildren<EditorUnit>())
            {
                if ((int)unit.ModelUnitClass < 0)
                {
                    OnError($"Unit \"{unit}\" does not have a class id", unit, throwOnError);
                }
            }
        }

        private static void ValidateUnitIds(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var unit in editorScenario.GetComponentsInChildren<EditorUnit>())
            {
                if (unit.Id < 0)
                {
                    OnError("All units require a valid (>0) id", unit, throwOnError);
                }
            }
        }

        private static void ValidateFactionIds(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var faction in editorScenario.GetComponentsInChildren<EditorFaction>())
            {
                if (faction.Id < 0)
                {
                    OnError("All factions require a valid (>0) id", faction, throwOnError);
                }
            }
        }

        private static void ValidateSectorIds(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var sector in editorScenario.GetComponentsInChildren<EditorSector>())
            {
                if (sector.Id < 0)
                {
                    OnError("All sectors require a valid (>0) id", sector, throwOnError);
                }
            }
        }

        private static void ValidatePersonIds(EditorScenario editorScenario, bool throwOnError)
        {
            foreach (var person in editorScenario.GetComponentsInChildren<EditorPerson>())
            {
                if (person.Id < 0)
                {
                    OnError("All people require a valid (>0) id", person, throwOnError);
                }
            }
        }

        private static void ValidateDuplicateSectors(EditorScenario editorScenario, bool throwOnError)
        {
            var sectors = editorScenario.GetComponentsInChildren<EditorSector>();
            var duplicates = sectors.Where(e => e.Id > -1).GroupBy(e => e.Id).Where(e => e.Count() > 1);
            if (duplicates.Any())
            {
                OnError("Duplicate sector ids found", duplicates.First().First(), throwOnError);
            }
        }

        private static void ValidateDuplicatePeople(EditorScenario editorScenario, bool throwOnError)
        {
            var people = editorScenario.GetComponentsInChildren<EditorPerson>();
            var duplicates = people.Where(e => e.Id > -1).GroupBy(e => e.Id).Where(e => e.Count() > 1);
            if (duplicates.Any())
            {
                OnError("Duplicate person ids found", duplicates.First().First(), throwOnError);
            }
        }

        private static void ValidateDuplicateUnits(EditorScenario editorScenario, bool throwOnError)
        {
            var units = editorScenario.GetComponentsInChildren<EditorUnit>();
            var duplicates = units.Where(e => e.Id > -1).GroupBy(e => e.Id).Where(e => e.Count() > 1);
            if (duplicates.Any())
            {
                OnError("Duplicate unit ids found", duplicates.First().First(), throwOnError);
            }
        }

        private static void ValidateDuplicateMessageIds(EditorScenario editorScenario, bool throwOnError)
        {
            var messages = editorScenario.GetComponentsInChildren<EditorPlayerMessage>();
            var duplicates = messages.Where(e => e.Id > -1).GroupBy(e => e.Id).Where(e => e.Count() > 1);
            if (duplicates.Any())
            {
                OnError("Duplicate message ids found", duplicates.First().First(), throwOnError);
            }
        }

        private static void ValidateDuplicateMissionIds(EditorScenario editorScenario, bool throwOnError)
        {
            var missions = editorScenario.GetComponentsInChildren<EditorMission>();
            var duplicates = missions.Where(e => e.Id > -1).GroupBy(e => e.Id).Where(e => e.Count() > 1);
            if (duplicates.Any())
            {
                OnError("Duplicate mission ids found", duplicates.First().First(), throwOnError);
            }
        }

        private static void ValidateDuplicateTriggerGroupIds(EditorScenario editorScenario, bool throwOnError)
        {
            var triggerGroups = editorScenario.GetComponentsInChildren<EditorTriggerGroup>();
            var duplicates = triggerGroups.Where(e => e.Id > -1).GroupBy(e => e.Id).Where(e => e.Count() > 1);
            if (duplicates.Any())
            {
                OnError("Duplicate trigger group ids found", duplicates.First().First(), throwOnError);
            }
        }

        private static void ValidateDuplicateMissionObjectiveIds(EditorScenario editorScenario, bool throwOnError)
        {
            var missionObjectives = editorScenario.GetComponentsInChildren<EditorMissionObjective>();
            var duplicates = missionObjectives.Where(e => e.Id > -1).GroupBy(e => e.Id).Where(e => e.Count() > 1);
            if (duplicates.Any())
            {
                OnError("Duplicate mission objective ids found", duplicates.First().First(), throwOnError);
            }
        }

        private static void ValidateDuplicateFactions(EditorScenario editorScenario, bool throwOnError)
        {
            var factions = editorScenario.GetComponentsInChildren<EditorFaction>();
            var duplicates = factions.Where(e => e.Id > -1).GroupBy(e => e.Id).Where(e => e.Count() > 1);
            if (duplicates.Any())
            {
                OnError("Duplicate faction ids found", duplicates.First().First(), throwOnError);
            }
        }

        private static void OnError(string message, Object context, bool throwOnError)
        {
            Debug.LogError(message, context);
            if (throwOnError)
            {
                throw new System.Exception("Validation throwing due to error");
            }
        }
    }
}
