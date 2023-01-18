using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Bounty;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public class AutoNameObjects
    {
        /// <summary>
        /// 
        /// </summary>
        public static void AutoNameAllObjects()
        {
            var editorScenario = GameObject.FindObjectOfType<EditorScenario>();
            if (editorScenario == null)
            {
                EditorUtility.DisplayDialog("Auto-assign object ids", $"Cannot find a {nameof(EditorScenario)} type object in the current scene", "Ok");
                return;
            }

            AutoNameAllObjects(editorScenario);

            Debug.Log("Finished auto-naming objects");
        }

        public static void AutoNameAllObjects(EditorScenario editorScenario)
        {
            AutoNameSectors(editorScenario);
            AutoNameUnits(editorScenario);
            AutoNameFactions(editorScenario);
            AutoNameFleets(editorScenario);
            AutoNamePeople(editorScenario);
            AutoNameCargo(editorScenario);
            AutoNameBountyItems(editorScenario);
        }

        public static void AutoNameCargo(EditorScenario editorScenario)
        {
            foreach (var editorCargo in editorScenario.GetComponentsInChildren<EditorCargo>())
            {
                AutoNameCargo(editorCargo);
            }
        }

        public static void AutoNameCargo(EditorCargo editorCargo)
        {
            editorCargo.gameObject.name = GetCargoName(editorCargo);
            EditorUtility.SetDirty(editorCargo);
        }

        public static string GetCargoName(EditorCargo editorCargo)
        {
            var cargoClass = editorCargo.ModelCargoClass.ToString().Replace("Cargo_", string.Empty);
            return $"Cargo_{cargoClass}_{editorCargo.Quantity}";
        }

        private static void AutoNameFleets(EditorScenario editorScenario)
        {
            foreach (var editorFleet in editorScenario.GetComponentsInChildren<EditorFleet>())
            {
                AutoNameFleet(editorFleet);
            }
        }

        public static void AutoNameFleet(EditorFleet editorFleet)
        {
            editorFleet.gameObject.name = GetEditorFleetName(editorFleet);
            EditorUtility.SetDirty(editorFleet);
        }

        private static string GetEditorFleetName(EditorFleet editorFleet)
        {
            var factionPostfix = "_NoFaction";
            if (editorFleet.Faction != null)
            {
                factionPostfix = $"_{(editorFleet.Faction != null ? editorFleet.Faction.CustomShortName : "NoFactionName")}";
            }

            if (!string.IsNullOrWhiteSpace(editorFleet.Designation))
            {
                return $"Fleet_{editorFleet.Designation}{factionPostfix}";
            }

            var name = GetEditorFleetNameFromContents(editorFleet);
            return $"Fleet_{name}{factionPostfix}";
        }

        private static string GetEditorFleetNameFromContents(EditorFleet editorFleet)
        {
            var units = editorFleet.GetComponentsInChildren<EditorUnit>();
            if (units.Length == 0)
            {
                return "Empty";
            }

            // Get the most common unit
            // TODO: It might be better here to get the most powerful unit
            var commonUnitGrouping = units.GroupBy(e => e.ModelUnitClass).OrderByDescending(e => e.Count()).First();
            var name = $"{commonUnitGrouping.First().ModelUnitClass.ToString()}";
            var count = commonUnitGrouping.Count();
            if (count > 1)
            {
                name += $"x{count}";
            }

            return name;
        }

        private static void AutoNameSectors(EditorScenario editorScenario)
        {
            foreach (var editorSector in editorScenario.GetComponentsInChildren<EditorSector>())
            {
                AutoNameSector(editorSector);
            }
        }

        public static void AutoNameSector(EditorSector editorSector)
        {
            editorSector.gameObject.name = $"Sector_{(!string.IsNullOrWhiteSpace(editorSector.Name) ? editorSector.Name : "Unnamed")}";
            EditorUtility.SetDirty(editorSector);
        }

        public static void AutoNamePeople(EditorScenario editorScenario)
        {
            foreach (var editorPerson in editorScenario.GetComponentsInChildren<EditorPerson>())
            {
                AutoNamePerson(editorPerson);
                EditorUtility.SetDirty(editorPerson);
            }
        }

        public static void AutoNamePerson(EditorPerson editorPerson)
        {
            editorPerson.gameObject.name = GetEditorPersonName(editorPerson);
        }

        public static void AutoNameUnits(EditorScenario editorScenario)
        {
            foreach (var editorSector in editorScenario.GetComponentsInChildren<EditorSector>())
            {
                foreach (var editorUnit in editorSector.GetComponentsInChildren<EditorUnit>())
                {
                    AutoNameUnit(editorUnit);
                    EditorUtility.SetDirty(editorSector);
                }
            }
        }

        public static void AutoNameUnit(EditorUnit editorUnit)
        {
            editorUnit.gameObject.name = GetEditorUnitName(editorUnit);
        }

        private static void AutoNameFactions(EditorScenario editorScenario)
        {
            foreach (var editorFaction in editorScenario.GetComponentsInChildren<EditorFaction>())
            {
                editorFaction.gameObject.name = GetEditorFactionName(editorFaction);
                EditorUtility.SetDirty(editorFaction);
            }
        }

        public static void AutoNameFactionRelation(EditorFactionRelation editorFactionRelation)
        {
            editorFactionRelation.gameObject.name = GetFactionRelationName(editorFactionRelation);
        }

        public static void AutoNameBountyItems(EditorScenario editorScenario)
        {
            foreach (var bountyItem in editorScenario.GetComponentsInChildren<EditorBountyBoardItem>())
            {
                bountyItem.name = GetBountyItemName(bountyItem);
            }
        }

        public static string GetBountyItemName(EditorBountyBoardItem bountyBoardItem)
        {
            return $"BountyBoardItem_{(bountyBoardItem.TargetPerson != null ? bountyBoardItem.TargetPerson.GetEditorName() : "NoTarget")}";
        }

        public static string GetFactionRelationName(EditorFactionRelation editorFactionRelation)
        {
            if (editorFactionRelation.OtherFaction == null)
            {
                return "FactionRelation_Invalid";
            }

            var targetFaction = !string.IsNullOrWhiteSpace(editorFactionRelation.OtherFaction.CustomShortName) ?
                editorFactionRelation.OtherFaction.CustomShortName : "NoName";
            return $"FactionRelation_{targetFaction}";
        }

        private static string GetEditorFactionName(EditorFaction editorFaction)
        {
            return $"Faction_{(!string.IsNullOrWhiteSpace(editorFaction.CustomShortName) ? editorFaction.CustomShortName : "NoName")}";
        }

        private static string GetEditorPersonName(EditorPerson editorPerson)
        {
            var factionName = editorPerson.Faction != null ? editorPerson.Faction.CustomName : "NoFaction";

            return $"Person_{editorPerson.GetEditorName()}_{factionName}";
        }

        private static string GetEditorUnitName(EditorUnit editorUnit)
        {
            if (editorUnit.ModelUnitClass.IsWormhole())
            {
                var wormholeData = editorUnit.GetComponent<EditorWormholeUnit>();
                if (wormholeData != null)
                {
                    return GetWormholeObjectName(wormholeData);
                }

                return $"Wormhole_MissingData";
            }

            var factionPostfix = string.Empty;
            if (editorUnit.Faction != null)
            {
                factionPostfix = $"_{(editorUnit.Faction != null ? editorUnit.Faction.CustomShortName : "NoFactionName")}";
            }

            if (editorUnit.ModelUnitClass.IsCargo())
            {
                var cargo = editorUnit.GetComponent<EditorCargoUnit>();
                if (cargo != null)
                {
                    return $"Cargo_{cargo.ModelCargoClass}_{cargo.Quantity}{factionPostfix}";
                }
                else
                {
                    return "Cargo_MissingData";
                }
            }

            if (editorUnit.ModelUnitClass.IsShipOrStation())
            {
                var name = editorUnit.ModelUnitClass.ToString();

                if (editorUnit.transform.GetComponentInImmediateParent<EditorHangarBay>() != null)
                {
                    name += " (Docked)";
                }

                if (editorUnit.Faction != null)
                {
                    return $"{name}{factionPostfix}";
                }

                return $"Abandoned_{name}";
            }

            return $"{editorUnit.ModelUnitClass.ToString()}";
        }

        public static void AutoNameWormhole(EditorWormholeUnit wormholeData)
        {
            wormholeData.gameObject.name = GetWormholeObjectName(wormholeData);
        }

        public static string GetWormholeObjectName(EditorWormholeUnit wormholeData)
        {
            var targetSector = wormholeData.GetActualTargetSector();

            var targetSectorName = targetSector != null ? targetSector.Name : "Nowhere";
            if (wormholeData.IsUnstable)
            {
                return $"UnstableWormhole_To_{targetSectorName}";
            }

            return $"Wormhole_To_{targetSectorName}";
        }
    }
}