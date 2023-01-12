using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit;
using Pixelfactor.IP.SavedGames.V2.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning
{
    public static class Spawn
    {
        public static EditorSector Sector(EditorScenario editorScenario)
        {
            return Sector(editorScenario, CustomSettings.GetOrCreateSettings().SectorPrefabPath);
        }

        public static EditorSector Sector(EditorScenario editorScenario, string prefabPath)
        {
            var sector = PrefabHelper.Instantiate<EditorSector>(prefabPath, editorScenario.GetSectorsRoot());
            EditSectorTool.Randomize(sector);
            return sector;
        }

        public static EditorFaction PlayerFaction(EditorScenario editorScenario)
        {
            return Faction(editorScenario, CustomSettings.GetOrCreateSettings().PlayerFactionPrefabPath);
        }

        public static EditorFaction Faction(EditorScenario editorScenario)
        {
            return Faction(editorScenario, CustomSettings.GetOrCreateSettings().FactionPrefabPath);
        }

        public static EditorFaction Faction(EditorScenario editorScenario, string prefabPath)
        {
            var faction = PrefabHelper.Instantiate<EditorFaction>(prefabPath, editorScenario.GetFactionsRoot());
            return faction;
        }

        public static EditorUnit Unit(EditorSector editorSector, ModelUnitClass modelUnitClass, string unitPrefabsPath)
        {
            var path = GetUnitPrefabPath(unitPrefabsPath, modelUnitClass);

            var unitAsset = AssetDatabase.LoadAssetAtPath<EditorUnit>(path);

            if (unitAsset == null)
                throw new System.Exception($"Could not find unit prefab at path: \"{path}\"");

            return Unit(editorSector, unitAsset);
        }

        public static EditorUnit Unit(EditorSector editorSector, EditorUnit unitAsset)
        {
            var unit = PrefabHelper.Instantiate(unitAsset, editorSector.transform);

            return unit;
        }

        public static string GetUnitPrefabPath(string basePath, ModelUnitClass modelUnitClass)
        {
            // This convention should work unless prefab names and paths are changed
            // The ModelUnitClass uses [UnitType]_[UnitName]
            // Another way to do this would be to bring all the prefabs into memory and lookup by ID - not sure how to do this in a robust way :S
            var split = modelUnitClass.ToString().Split("_", 2, System.StringSplitOptions.RemoveEmptyEntries);

            return $"{basePath.TrimEnd('/')}/{split[0]}/Unit_{modelUnitClass}.prefab";
        }

        public static EditorFleet FleetForUnits(IEnumerable<EditorUnit> editorUnits)
        {
            var settings = CustomSettings.GetOrCreateSettings();
            var fleetPath = settings.FleetPrefabPath;
            var fleet = PrefabHelper.Instantiate<EditorFleet>(fleetPath);
            var firstUnit = editorUnits.First();
            fleet.transform.position = firstUnit.transform.position;
            fleet.transform.SetParent(firstUnit.GetSector().transform);
            fleet.Faction = firstUnit.Faction;
            EditorUtility.SetDirty(fleet);

            foreach (var unit in editorUnits)
            {
                var npc = PilotForUnitIfNone(unit);

                // TODO: If any of the units are docked, we shouldn't nest the unit underneath the fleet
                unit.transform.SetParent(fleet.transform);
            }

            return fleet;
        }

        public static EditorNpcPilot PilotForUnitIfNone(EditorUnit editorUnit)
        {
            var npc = editorUnit.GetComponentInChildren<EditorNpcPilot>();
            if (npc == null)
            {
                return PilotForUnit(editorUnit);
            }

            return null;
        }

        public static EditorNpcPilot PilotForUnit(EditorUnit editorUnit)
        {
            var settings = CustomSettings.GetOrCreateSettings();
            var npc = PrefabHelper.Instantiate<EditorNpcPilot>(settings.NpcPrefabPath, editorUnit.transform);
            return npc;
        }
    }
}
