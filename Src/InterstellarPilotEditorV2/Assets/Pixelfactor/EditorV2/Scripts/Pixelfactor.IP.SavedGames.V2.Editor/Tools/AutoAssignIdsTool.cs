﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Missions;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting;
using Pixelfactor.IP.SavedGames.V2.Editor.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public class AutoAssignIdsTool
    {
        /// <summary>
        /// No id should be used less than this value.
        /// </summary>
        public static int BASE_ID = 100000;

        [MenuItem("Window/IP Editor V2/Tools/Ids/Auto-assign object Ids")]
        public static void AutoAssignIdsMenuItem()
        {
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();

            AutoAssignIds(editorSavedGame);

            Debug.Log("Finished auto-assigning ids");
        }

        [MenuItem("Window/IP Editor V2/Tools/Ids/Clear all Ids")]
        public static void ClearIdsMenuItem()
        {
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();

            ClearAllIds(editorSavedGame);

            Debug.Log("Finished clearing ids");
        }

        [MenuItem("Window/IP Editor V2/Tools/Ids/Reassign all Ids")]
        public static void ReassignIdsMenuItem()
        {
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();

            ClearAllIds(editorSavedGame);
            AutoAssignIds(editorSavedGame);

            Debug.Log("Finished reassigning ids");
        }

        public static void AutoAssignIds(EditorSavedGame editorSavedGame)
        {
            var missions = editorSavedGame.GetComponentsInChildren<EditorMission>();
            foreach (var mission in missions)
            {
                if (mission.Id < 0)
                {
                    mission.Id = NewMissionId(missions);
                    EditorUtility.SetDirty(mission);
                }
            }

            var missionObjectives = editorSavedGame.GetComponentsInChildren<EditorMissionObjective>();
            foreach (var missionObjective in missionObjectives)
            {
                if (missionObjective.Id < 0)
                {
                    missionObjective.Id = NewMissionObjectiveId(missionObjectives);
                    EditorUtility.SetDirty(missionObjective);
                }
            }

            var triggerGroups = editorSavedGame.GetComponentsInChildren<EditorTriggerGroup>();
            foreach (var triggerGroup in triggerGroups)
            {
                if (triggerGroup.Id < 0)
                {
                    triggerGroup.Id = NewTriggerGroupId(triggerGroups);
                    EditorUtility.SetDirty(triggerGroup);
                }
            }

            var units = editorSavedGame.GetComponentsInChildren<EditorUnit>();
            foreach (var unit in units)
            {
                if (unit.Id < 0)
                {
                    unit.Id = NewUnitId(units);
                    EditorUtility.SetDirty(unit);
                }
            }

            var factions = editorSavedGame.GetComponentsInChildren<EditorFaction>();
            foreach (var faction in factions)
            {
                if (faction.Id < 0)
                {
                    faction.Id = NewFactionId(factions);
                    EditorUtility.SetDirty(faction);
                }
            }

            var sectors = editorSavedGame.GetComponentsInChildren<EditorSector>();
            foreach (var sector in sectors)
            {
                if (sector.Id < 0)
                {
                    sector.Id = NewSectorId(sectors);
                    EditorUtility.SetDirty(sector);
                }
            }

            var people = editorSavedGame.GetComponentsInChildren<EditorPerson>();
            foreach (var person in people)
            {
                if (person.Id < 0)
                {
                    person.Id = NewPersonId(people);
                    EditorUtility.SetDirty(person);
                }
            }

            var fleets = editorSavedGame.GetComponentsInChildren<EditorFleet>();
            foreach (var fleet in fleets)
            {
                if (fleet.Id < 0)
                {
                    fleet.Id = NewFleetId(fleets);
                    EditorUtility.SetDirty(fleet);
                }
            }

            var orders = editorSavedGame.GetComponentsInChildren<EditorFleetOrderCommon>();
            foreach (var order in orders)
            {
                if (order.Id < 0)
                {
                    order.Id = NewOrderId(orders);
                    EditorUtility.SetDirty(order);
                }
            }

            var messages = editorSavedGame.GetComponentsInChildren<EditorPlayerMessage>();
            foreach (var message in messages)
            {
                if (message.Id < 0)
                {
                    message.Id = NewMessageId(messages);
                    EditorUtility.SetDirty(message);
                }
            }
        }

        public static void ClearAllIds(EditorSavedGame editorSavedGame)
        {
            var units = editorSavedGame.GetComponentsInChildren<EditorUnit>();
            foreach (var unit in units)
            {
                unit.Id = -1;
                EditorUtility.SetDirty(unit);
            }

            var factions = editorSavedGame.GetComponentsInChildren<EditorFaction>();
            foreach (var faction in factions)
            {
                faction.Id = -1;
                EditorUtility.SetDirty(faction);
            }

            var sectors = editorSavedGame.GetComponentsInChildren<EditorSector>();
            foreach (var sector in sectors)
            {
                sector.Id = -1;
                EditorUtility.SetDirty(sector);
            }

            var people = editorSavedGame.GetComponentsInChildren<EditorPerson>();
            foreach (var person in people)
            {
                person.Id = -1;
                EditorUtility.SetDirty(person);
            }

            var fleets = editorSavedGame.GetComponentsInChildren<EditorFleet>();
            foreach (var fleet in fleets)
            {
                fleet.Id = -1;
                EditorUtility.SetDirty(fleet);
            }

            var orders = editorSavedGame.GetComponentsInChildren<EditorFleetOrderCommon>();
            foreach (var order in orders)
            {
                order.Id = -1;
                EditorUtility.SetDirty(order);
            }

            var messages = editorSavedGame.GetComponentsInChildren<EditorPlayerMessage>();
            foreach (var message in messages)
            {
                message.Id = -1;
                EditorUtility.SetDirty(message);
            }
        }

        private static int NewOrderId(IEnumerable<EditorFleetOrderCommon> orders)
        {
            return Mathf.Max(BASE_ID, orders.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }

        private static int NewMessageId(IEnumerable<EditorPlayerMessage> messages)
        {
            return Mathf.Max(BASE_ID, messages.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }

        private static int NewFleetId(IEnumerable<EditorFleet> fleets)
        {
            return Mathf.Max(BASE_ID, fleets.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }

        public static int NewUnitId(IEnumerable<EditorUnit> units)
        {
            return Mathf.Max(BASE_ID, units.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }

        public static int NewTriggerGroupId(IEnumerable<EditorTriggerGroup> units)
        {
            return Mathf.Max(BASE_ID, units.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }

        public static int NewMissionId(IEnumerable<EditorMission> missions)
        {
            return Mathf.Max(BASE_ID, missions.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }

        public static int NewMissionObjectiveId(IEnumerable<EditorMissionObjective> missions)
        {
            return Mathf.Max(BASE_ID, missions.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }

        public static int NewFactionId(IEnumerable<EditorFaction> factions)
        {
            return Mathf.Max(BASE_ID, factions.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }

        public static int NewSectorId(IEnumerable<EditorSector> sectors)
        {
            return Mathf.Max(BASE_ID, sectors.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }

        public static int NewPersonId(IEnumerable<EditorPerson> people)
        {
            return Mathf.Max(BASE_ID, people.Select(e => e.Id).DefaultIfEmpty().Max()) + 1;
        }
    }
}
