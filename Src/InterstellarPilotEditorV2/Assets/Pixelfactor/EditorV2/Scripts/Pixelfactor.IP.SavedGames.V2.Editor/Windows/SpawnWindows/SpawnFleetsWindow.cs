using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
using Pixelfactor.IP.SavedGames.V2.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows
{
    public class SpawnFleetsWindow
    {
        private int spawnFleetShipCount = 8;
        private string spawnFleetName = null;
        private float spawnFleetMinShipSize = 0.0f;
        private float spawnFleetMaxShipSize = 1.0f;
        private EditorFactionStrategy spawnFleetShipTypes = EditorFactionStrategy.War;

        public void Draw(ref EditorFaction spawnFaction)
        {
            DrawSpawnNewFleetOptions(ref spawnFaction);
            DrawSpawnFleetFromExisting();
        }

        public void DrawSpawnNewFleetOptions(ref EditorFaction spawnFaction)
        {
            GuiHelper.Subtitle("Spawn fleet", "Spawns a fleet of ships");
            this.spawnFleetName = EditorGUILayout.TextField(new GUIContent("Fleet name", "Optional name to give to the fleet"), this.spawnFleetName, GUILayout.ExpandWidth(true));

            var factionContent = new GUIContent("Spawn faction", "The faction that the spawned unit will be assigned to");
            spawnFaction = (EditorFaction)EditorGUILayout.ObjectField(factionContent, spawnFaction, typeof(EditorFaction), allowSceneObjects: true);

            EditorGUILayout.PrefixLabel(new GUIContent("Ship count", "The number of ships in the fleet"));
            this.spawnFleetShipCount = EditorGUILayout.IntSlider(this.spawnFleetShipCount, 1, 8, GUILayout.ExpandWidth(false));

            var sector = Selector.GetSingleSelectedSectorOrNull();
            var canSpawnFleet = spawnFaction != null && sector != null;

            this.spawnFleetShipTypes = (EditorFactionStrategy)EditorGUILayout.EnumFlagsField(
                new GUIContent("Ship types", "The type of ships to include in the fleet"),
                this.spawnFleetShipTypes,
                GUILayout.ExpandWidth(false));

            EditorGUILayout.MinMaxSlider(
                new GUIContent("Ship size", "The min and max size of the ships to spawn"),
                ref this.spawnFleetMinShipSize,
                ref this.spawnFleetMaxShipSize,
                0.0f,
                1.0f);

            EditorGUI.BeginDisabledGroup(!canSpawnFleet);
            if (GUILayout.Button(
                new GUIContent(
                    $"Spawn",
                    $"Creates a new fleet"),
                GuiHelper.ButtonLayout))
            {
                SpawnNewFleet(sector, spawnFaction);
            }
            EditorGUI.EndDisabledGroup();
        }

        public void SpawnNewFleet(EditorSector sector, EditorFaction faction)
        {
            var shipPrefabs = GameObjectHelper.GetPrefabsOfTypeFromPath<EditorUnit>(
                CustomSettings.GetOrCreateSettings().UnitPrefabsPath.Trim('/') + "/Ship").ToList();

            var newFleetUnits = new List<EditorUnit>();
            if (shipPrefabs.Count > 0)
            {
                for (int i = 0; i < this.spawnFleetShipCount; i++)
                {
                    newFleetUnits.Add(SpawnFleetUnit(sector, faction, shipPrefabs));
                }
            }

            if (newFleetUnits.Count == 0)
            {
                EditorUtility.DisplayDialog("Spawn fleet", "No ships were found that match the parameters", "OK");
                return;
            }

            var newFleet = Spawn.FleetForUnits(newFleetUnits);
            newFleet.Designation = this.spawnFleetName;
            EditorUtility.SetDirty(newFleet);
            AutoNameObjects.AutoNameFleet(newFleet);
            Selection.activeObject = newFleet.gameObject;
        }

        public void DrawSpawnFleetFromExisting()
        {
            GuiHelper.Subtitle("Spawn fleet from selected", "Spawns a fleet from existing units");

            var canSpawnFleetFromSelected = CanSpawnFleetForSelectedUnits(out List<EditorUnit> selectedUnits);
            this.spawnFleetName = EditorGUILayout.TextField(new GUIContent("Fleet name", "Optional name to give to the fleet"), this.spawnFleetName, GUILayout.ExpandWidth(true));

            EditorGUI.BeginDisabledGroup(!canSpawnFleetFromSelected);

            if (GUILayout.Button(
                new GUIContent(
                    "Fleet for selected units",
                    "Creates a fleet for the selected units"),
                GuiHelper.ButtonLayout))
            {
                var newFleet = Spawn.FleetForUnits(selectedUnits);
                newFleet.Designation = this.spawnFleetName;
                EditorUtility.SetDirty(newFleet);
                AutoNameObjects.AutoNameFleet(newFleet);

                Selection.activeObject = newFleet.gameObject;
            }

            EditorGUI.EndDisabledGroup();

            if (!canSpawnFleetFromSelected)
            {
                GuiHelper.HelpPrompt("Ensure that all selected objects are ships and that they have a faction, and aren't already part of a fleet");
            }
        }

        private EditorUnit SpawnFleetUnit(
            EditorSector sector, 
            EditorFaction faction,
            IEnumerable<EditorUnit> prefabs)
        {
            var randomFleetShipClass = GetRandomSpawnFleetShipClass(prefabs);
            var unit = Spawn.Unit(sector, randomFleetShipClass, CustomSettings.GetOrCreateSettings().UnitPrefabsPath);
            unit.Faction = faction;
            unit.transform.position = SpawnWindowHelper.GetNewUnitSpawnPosition(sector, unit.GetCollisionRadius());

            Physics.autoSimulation = false;
            Physics.Simulate(0.1f);
            EditorUtility.SetDirty(unit);
            return unit;
        }

        private ModelUnitClass GetRandomSpawnFleetShipClass(IEnumerable<EditorUnit> prefabs)
        {
            var ships = prefabs
                .Where(e => (e.EditorShipPurpose & this.spawnFleetShipTypes) != EditorFactionStrategy.None)
                .Where(e => e.GetRelativeSize() >= this.spawnFleetMinShipSize && e.GetRelativeSize() <= this.spawnFleetMaxShipSize).ToList();
            if (ships.Any())
            {
                return ships.GetRandom().ModelUnitClass;
            }

            return ModelUnitClass.Ship_ShuttleA;
        }

        public static bool CanSpawnFleetForSelectedUnits(out List<EditorUnit> units)
        {
            if (Selector.TryGetSelectedRootUnits(out units))
            {
                if (units.Count > 0)
                {
                    var firstFaction = units[0].Faction;

                    if (firstFaction != null)
                    {
                        if (units.All(e => e.Faction == firstFaction && e.GetFleet() == null))
                        {
                            return true;
                        }
                    }
                }
            }

            units = null;
            return false;
        }
    }
}
