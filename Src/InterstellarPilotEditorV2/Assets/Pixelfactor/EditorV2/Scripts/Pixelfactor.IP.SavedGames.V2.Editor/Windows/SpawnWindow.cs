using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
using Pixelfactor.IP.SavedGames.V2.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class SpawnWindow
    {
        public const int SpawnShipId = 0;
        public const int SpawnStationId = 1;
        public const int SpawnAsteroidsId = 2;
        public const int SpawnFleetsId = 3;

        private int currentTab = SpawnShipId;
        private EditorFaction spawnFaction = null;
        private int spawnFleetShipCount = 8;
        private string spawnFleetName = null;
        private EditorFactionStrategy spawnFleetShipTypes = EditorFactionStrategy.War;

        private float spawnFleetMinShipSize = 0.0f;
        private float spawnFleetMaxShipSize = 1.0f;

        public void Draw()
        {

            currentTab = GUILayout.Toolbar(currentTab, new string[] { "Ship", "Station", "Asteroid", "Fleet" });

            switch (currentTab)
            {
                case SpawnShipId:
                    {
                        ShowSpawnOptionsAndSector("Ship");
                    }
                    break;
                case SpawnStationId:
                    {
                        ShowSpawnOptionsAndSector("Station");
                    }
                    break;
                case SpawnAsteroidsId:
                    {
                        DrawSpawnAsteroidOptions();

                        EditorGUILayout.Space(30);

                        ShowSpawnOptionsAndSector("Asteroid", allowFaction: false);
                        ShowSpawnOptions("AsteroidCluster", allowFaction: false);
                    }
                    break;
                case SpawnFleetsId:
                    {
                        DrawSpawnFleetOptions();
                    }
                    break;
            }

            EditorGUI.EndDisabledGroup();
        }

        private static bool CanSpawnFleetForSelectedUnits(out List<EditorUnit> units)
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

        private void DrawSpawnFleetOptions()
        {
            DrawSpawnNewFleetOptions();
            DrawSpwnFleetFromExisting();
        }

        private void DrawSpawnNewFleetOptions()
        {
            GuiHelper.Subtitle("Spawn fleet", "Spawns a fleet of ships");
            this.spawnFleetName = EditorGUILayout.TextField(new GUIContent("Fleet name", "Optional name to give to the fleet"), this.spawnFleetName, GUILayout.ExpandWidth(true));

            var factionContent = new GUIContent("Spawn faction", "The faction that the spawned unit will be assigned to");
            this.spawnFaction = (EditorFaction)EditorGUILayout.ObjectField(factionContent, this.spawnFaction, typeof(EditorFaction), allowSceneObjects: true);

            EditorGUILayout.PrefixLabel(new GUIContent("Ship count", "The number of ships in the fleet"));
            this.spawnFleetShipCount = EditorGUILayout.IntSlider(this.spawnFleetShipCount, 1, 8, GUILayout.ExpandWidth(false));

            var sector = Selector.GetSingleSelectedSectorOrNull();
            var canSpawnFleet = this.spawnFaction != null && sector != null;

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
                SpawnNewFleet(sector);
            }
            EditorGUI.EndDisabledGroup();
        }

        private void SpawnNewFleet(EditorSector sector)
        {
            var shipPrefabs = GameObjectHelper.TryGetUnityObjectsOfTypeFromPath<EditorUnit>(
                CustomSettings.GetOrCreateSettings().UnitPrefabsPath.Trim('/') + "/Ship").ToList();

            var newFleetUnits = new List<EditorUnit>();
            if (shipPrefabs.Count > 0)
            {
                for (int i = 0; i < this.spawnFleetShipCount; i++)
                {
                    newFleetUnits.Add(SpawnFleetUnit(sector, shipPrefabs));
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

        private void DrawSpwnFleetFromExisting()
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

        private EditorUnit SpawnFleetUnit(EditorSector sector, IEnumerable<EditorUnit> prefabs)
        {
            var randomFleetShipClass = GetRandomSpawnFleetShipClass(prefabs);
            var unit = Spawn.Unit(sector, randomFleetShipClass, CustomSettings.GetOrCreateSettings().UnitPrefabsPath);
            unit.Faction = this.spawnFaction;
            unit.transform.position = GetNewUnitSpawnPosition(sector, unit.GetCollisionRadius());

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

        private static void DrawSpawnAsteroidOptions()
        {
            var sectors = Selector.GetInParents<EditorSector>();

            var hasSectors = sectors.Any();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Sector", WindowHelper.DescribeSectors(sectors));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!hasSectors);

            if (GUILayout.Button(
                new GUIContent(
                    "Spawn asteroids in all sectors",
                    "Creates asteroids inside asteroid clusters of every sector"),
                GuiHelper.ButtonLayout))
            {
                var count = AsteroidSpawnTool.SpawnAsteroidsInSectors(sectors);

                var message = count > 0 ?
                    $"Finished creating {count} asteroids" :
                    "No asteroids were created. Ensure the selected sectors have asteroid clusters or aren't already filled with asteroids";

                EditorUtility.DisplayDialog("Spawn asteroids", message, "OK");
            }
        }

        private void ShowSpawnOptionsAndSector(string subDirectory, bool allowFaction = true)
        {
            var sector = Selector.GetSingleSelectedSectorOrNull();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Spawn Sector", WindowHelper.DescribeSectors(sector));
            EditorGUI.EndDisabledGroup();

            ShowSpawnOptions(subDirectory, allowFaction);
        }

        private void ShowSpawnOptions(string subDirectory, bool allowFaction)
        {
            var sector = Selector.GetSingleSelectedSectorOrNull();

            var canSpawn = sector != null;

            EditorGUI.BeginDisabledGroup(!canSpawn);

            if (allowFaction)
            {
                var factionContent = new GUIContent("Spawn faction", "The faction that the spawned unit will be assigned to");
                this.spawnFaction = (EditorFaction)EditorGUILayout.ObjectField(factionContent, this.spawnFaction, typeof(EditorFaction), allowSceneObjects: true);
            }

            var settings = CustomSettings.GetOrCreateSettings();
            var prefabs = GameObjectHelper.TryGetUnityObjectsOfTypeFromPath<EditorUnit>(settings.UnitPrefabsPath.Trim('/') + "/" + subDirectory).ToList();

            if (prefabs.Count > 0)
            {

                var viewWidth = EditorGUIUtility.currentViewWidth;
                var columnCount = Mathf.Max(1, Mathf.FloorToInt(viewWidth / 200));

                var i = 0;
                while (i < prefabs.Count)
                {
                    var unitPrefab = prefabs[i];
                    if (columnCount > 1)
                    {
                        if (i == 0)
                        {
                            EditorGUILayout.BeginHorizontal();
                        }
                        else
                        {
                            var currentColumnIndex = i % columnCount;
                            if (currentColumnIndex == 0)
                            {
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                            }
                        }
                    }

                    DrawSpawnPrefabButton(sector, unitPrefab, this.spawnFaction);

                    i++;
                }

                if (columnCount > 1)
                {
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUI.EndDisabledGroup();
        }

        Vector3 GetSceneViewCenter()
        {
            return GetCurrentScenePositionInScene(SceneView.lastActiveSceneView.position.center);
        }

        Vector3 GetCurrentScenePositionInScene()
        {
            return GetCurrentScenePositionInScene(Event.current.mousePosition);
        }

        Vector3 GetCurrentScenePositionInScene(Vector2 screenSpace)
        {
            Vector3 mousePosition = Event.current.mousePosition;
            var placeObject = HandleUtility.PlaceObject(mousePosition, out var newPosition, out var normal);
            var p = placeObject ? newPosition : HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10);
            p.y = 0.0f;
            return p;
        }

        private Vector3 GetNewUnitSpawnPosition(EditorSector sector, float radius)
        {
            var initialPosition = GetCurrentScenePositionInScene();

            var newPosition = SpawnPositionFinder.FindPositionOrNull(sector, initialPosition, radius);
            if (newPosition.HasValue)
            {
                return newPosition.Value;
            }

            return sector.transform.position;
        }

        private void DrawSpawnPrefabButton(EditorSector sector, EditorUnit unitPrefab, EditorFaction editorFaction)
        {
            if (GUILayout.Button(
                new GUIContent(
                    $"Spawn {unitPrefab.GetEditorName()}",
                    $"Creates a {unitPrefab.GetEditorName()}"),
                GuiHelper.ButtonLayout))
            {
                if (sector == null)
                {
                    EditorUtility.DisplayDialog("Spawn", "Select a sector first", "OK");
                }
                else
                {
                    var unit = Spawn.Unit(sector, unitPrefab);
                    if (unit.CanHaveFaction())
                    {
                        unit.Faction = editorFaction;
                    }

                    unit.transform.position = GetNewUnitSpawnPosition(sector, unit.GetCollisionRadius());

                    Selection.objects = new GameObject[] { unit.gameObject };

                    // Auto-frame on spawned object
                    //var viewSize = 100.0f;;
                    //SceneView.lastActiveSceneView.Frame(new Bounds(unit.transform.position, new Vector3(viewSize, viewSize, viewSize)), false);
                }
            }
        }
    }
}
