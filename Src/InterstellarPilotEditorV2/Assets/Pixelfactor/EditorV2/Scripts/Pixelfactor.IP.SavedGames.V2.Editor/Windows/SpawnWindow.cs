﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Planets;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
using Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows;
using Pixelfactor.IP.SavedGames.V2.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class SpawnWindow
    {
        public const int SpawnShipId = 3;
        public const int SpawnStationId = 4;
        
        public const int SpawnFleetsId = 5;

        public const int SpawnPlanetId = 0;
        public const int SpawnAsteroidClustersId = 1;
        public const int SpawnAsteroidsId = 2;

        private int currentTab = 0;
        private EditorFaction spawnFaction = null;

        private bool autoPositionPlanets = true;

        private SpawnFleets spawnFleetsWindow = new SpawnFleets();

        public void Draw()
        {

            currentTab = GUILayout.Toolbar(currentTab, new string[] { "Planet", "Cluster", "Asteroid", "Ship", "Station", "Fleet" });

            switch (currentTab)
            {
                case SpawnPlanetId:
                    {
                        var settings = CustomSettings.GetOrCreateSettings();
                        var planetPrefabPath = settings.UnitPrefabsPath.TrimEnd('/') + "/" + "Planet";
                        var allPlanetPrefabs = GameObjectHelper.GetPrefabsOfTypeFromPath<EditorUnit>(planetPrefabPath).Where(e => e.ModelUnitClass != ModelUnitClass.Planet_Earth).ToList();
                        var planetPrefabs = allPlanetPrefabs.Where(e => !e.name.Contains("moon", System.StringComparison.InvariantCultureIgnoreCase)).ToList();
                        var moonPrefabs = allPlanetPrefabs.Where(e => e.name.Contains("moon", System.StringComparison.InvariantCultureIgnoreCase)).ToList();

                        DrawPlanetAutoSpawnOptions(allPlanetPrefabs, planetPrefabs, moonPrefabs);

                        DrawSpawnPlanetOptions(allPlanetPrefabs, planetPrefabs, moonPrefabs);

                        DrawDeletePlanetOptions();
                    }
                    break;
                case SpawnAsteroidClustersId:
                    {
                        GuiHelper.Subtitle("Spawn asteroid clusters", "Create asteroid clusters in selected sectors");
                        ShowSpawnOptions("AsteroidCluster", allowFaction: false);
                    }
                    break;
                case SpawnAsteroidsId:
                    {
                        DrawAsteroidAutoSpawnOptions();

                        EditorGUILayout.Space(30);

                        GuiHelper.Subtitle("Spawn single asteroid", "Spawn an asteroid in a specific sector");
                        ShowSpawnOptionsAndSector("Asteroid", allowFaction: false);


                    }
                    break;
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
                case SpawnFleetsId:
                    {
                        this.spawnFleetsWindow.Draw(ref this.spawnFaction);
                    }
                    break;
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DrawDeletePlanetOptions()
        {
            GuiHelper.Subtitle("Delete planets", "Deletes planets from the selected sectors");
            var selectedSectors = Selector.GetInParents<EditorSector>();
            if (selectedSectors.Count() == 0)
            {
                var savedGame = SavedGameUtil.FindSavedGame();
                selectedSectors = savedGame.GetComponentsInChildren<EditorSector>();
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Target sector", WindowHelper.DescribeSectors(selectedSectors));
            EditorGUI.EndDisabledGroup();

            var canDelete = selectedSectors.Count() > 0;

            EditorGUI.BeginDisabledGroup(!canDelete);
            if (GUILayout.Button(
                new GUIContent(
                    $"Delete planets",
                    $"Deletes planets and moons from the selected sectors"),
                GuiHelper.ButtonLayout))
            {
                foreach (var sector in selectedSectors)
                {
                    foreach (var planet in sector.GetComponentsInChildren<EditorPlanet>())
                    {
                        GameObject.DestroyImmediate(planet.gameObject);
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawSpawnPlanetOptions(
            IEnumerable<EditorUnit> allPlanetPrefabs, 
            IEnumerable<EditorUnit> planetPrefabs,
            IEnumerable<EditorUnit> moonPrefabs)
        {
            GuiHelper.Subtitle("Spawn planets", "Spawns planets in selected sectors");

            var selectedSectors = Selector.GetInParents<EditorSector>();
            if (selectedSectors.Count() == 0)
            {
                var savedGame = SavedGameUtil.FindSavedGame();
                selectedSectors = savedGame.GetComponentsInChildren<EditorSector>();
            }

            var canSpawnPlanet = selectedSectors.Count() > 0;

            var settings = CustomSettings.GetOrCreateSettings();

            EditorGUILayout.PrefixLabel(new GUIContent("Auto-position", "Whether to randomly position planets in a sector in the same way that the game-engine would"));
            this.autoPositionPlanets = EditorGUILayout.Toggle(this.autoPositionPlanets, GUILayout.ExpandWidth(false));

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Spawn sector", WindowHelper.DescribeSectors(selectedSectors));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!canSpawnPlanet);
            if (GUILayout.Button(
                new GUIContent(
                    $"Spawn random planet",
                    $"Creates a random planet in selected sectors"),
                GuiHelper.ButtonLayout))
            {
                OnSpawnPlanetClick(selectedSectors, planetPrefabs, autoPositionPlanets, settings);
            }

            GuiHelper.FlowButtons(planetPrefabs.Select(planetPrefab => new GuiHelper.ButtonInfo
            {
                Text = $"Spawn {planetPrefab.GetEditorName()}",
                Description = $"Spawns a planet in selected sectors",
                OnClick = () =>
                {
                    foreach (var sector in selectedSectors)
                    {
                        SpawnNewPlanet(planetPrefab, sector, autoPositionPlanets, settings);
                    }
                }

            }).ToList());

            GuiHelper.Subtitle("Spawn moons", "Spawns moons in selected sectors");
            GuiHelper.HelpPrompt("Experimental - moons are currently treated the same as planets");

            GuiHelper.FlowButtons(moonPrefabs.Select(planetPrefab => new GuiHelper.ButtonInfo
            {
                Text = $"Spawn {planetPrefab.GetEditorName()}",
                Description = $"Spawns a planet in selected sectors",
                OnClick = () =>
                {
                    foreach (var sector in selectedSectors)
                    {
                        SpawnNewPlanet(planetPrefab, sector, autoPositionPlanets, settings);
                    }
                }

            }).ToList());

            EditorGUI.EndDisabledGroup();
        }

        private void DrawPlanetAutoSpawnOptions(
    IEnumerable<EditorUnit> allPlanetPrefabs,
    IEnumerable<EditorUnit> planetPrefabs,
    IEnumerable<EditorUnit> moonPrefabs)
        {
            GuiHelper.Subtitle("Auto-spawn planets", "Spawn planets in all sectors based on settings");

            var sectors = GetAllSectors();
            var hasSectors = sectors.Any();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Sector", WindowHelper.DescribeSectors(sectors));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!hasSectors);

            if (GUILayout.Button(
                new GUIContent(
                    "Auto spawn",
                    "Creates planets automatically in all sectors"),
                GuiHelper.ButtonLayout))
            {
                var settings = CustomSettings.GetOrCreateSettings();
                var sectorsToSpawnPlanets = PlanetSpawnTool.GetNewPlanetSectors(sectors, settings);

                // Find all existing planets - use this to try and spawn unique ones
                var allPlanets = SavedGameUtil.FindSavedGameOrErrorOut().GetComponentsInChildren<EditorPlanet>().Select(e => e.GetComponent<EditorUnit>()).ToList();
                var count = 0;
                foreach (var sector in sectorsToSpawnPlanets)
                {
                    Debug.Log($"Spawning planet in {sector.Name}");
                    var newPlanet = SpawnNewPlanet(sector, autoPositionPlanet: true, planetPrefabs, settings, allPlanets);
                    if (newPlanet != null)
                    {
                        allPlanets.Add(newPlanet);
                        count++;
                    }
                }

                //var count = PlanetSpawnTool.SpawnPlanetsInSectors(sectors, );

                var message = count > 0 ?
                    $"Finished creating {count} planets" :
                    "No planets were created. Ensure that planet prefabs can be found";

                EditorUtility.DisplayDialog("Spawn planets", message, "OK");
            }

            EditorGUI.EndDisabledGroup();
        }

        private void OnSpawnPlanetClick(
            IEnumerable<EditorSector> selectedSectors, 
            IEnumerable<EditorUnit> planetPrefabs, 
            bool autoPositionPlanet,
            CustomSettings customSettings)
        {
            if (planetPrefabs.Count() == 0)
            {
                Debug.LogError("No planet prefabs were found");
                return;
            }

            // Find all existing planets - use this to try and spawn unique ones
            var allPlanets = SavedGameUtil.FindSavedGameOrErrorOut().GetComponentsInChildren<EditorPlanet>().Select(e => e.GetComponent<EditorUnit>()).ToList();

            foreach (var sector in selectedSectors)
            {
                var newPlanet = SpawnNewPlanet(sector, autoPositionPlanet, planetPrefabs, customSettings, allPlanets);

                if (newPlanet != null)
                { 
                    allPlanets.Add(newPlanet);
                }
            }
        }

        private EditorUnit SpawnNewPlanet(
            EditorSector sector,
            bool autoPositionPlanet, 
            IEnumerable<EditorUnit> planetPrefabs, 
            CustomSettings customSettings,
            List<EditorUnit> allPlanets)
        {
            var randomPlanetPrefab = GetRandomPlanetPrefab(planetPrefabs, allPlanets);
            return SpawnNewPlanet(randomPlanetPrefab, sector, autoPositionPlanet, customSettings);
        }

        private EditorUnit GetRandomPlanetPrefab(IEnumerable<EditorUnit> planetPrefabs, List<EditorUnit> allPlanets)
        {
            return planetPrefabs.OrderBy(e => GetCountOfPlanetClass(e, allPlanets) + (Random.value * 0.2f)).FirstOrDefault();
        }

        private int GetCountOfPlanetClass(EditorUnit editorUnit, List<EditorUnit> allPlanets)
        {
            var count = 0;
            foreach (var planet in allPlanets)
            {
                if (planet.ModelUnitClass == editorUnit.ModelUnitClass)
                {
                    count++;
                }
            }

            return count;
        }

        private EditorUnit SpawnNewPlanet(EditorUnit prefab, EditorSector sector, bool autoPositionPlanet, CustomSettings settings)
        {
            var planetUnit = PrefabHelper.Instantiate(prefab, sector.transform);

            if (autoPositionPlanet)
            {
                AutoPositionPlanetTool.AutoPositionPlanet(planetUnit, sector, settings);
            }
            else
            { 
                planetUnit.transform.position = SpawnWindowHelper.GetNewUnitSpawnPosition(sector, 1000.0f);
            }

            AutoNameObjects.AutoNameUnit(planetUnit);

            return planetUnit;
        }

        private static List<EditorSector> GetSelectedOrAllSectors()
        {
            var sectors = Selector.GetInParents<EditorSector>();
            if (sectors.Any())
            {
                return sectors.ToList();
            }

            return GetAllSectors();
        }

        public static List<EditorSector> GetAllSectors()
        {
            var savedGame = SavedGameUtil.FindSavedGame();
            if (savedGame != null)
            {
                return savedGame.GetSectors().ToList();
            }

            return new EditorSector[0].ToList();
        }

        private static void DrawAsteroidAutoSpawnOptions()
        {
            GuiHelper.Subtitle("Spawn asteroids", "Spawn asteroids in selected sectors based on existing asteroid clusters");
            var sectors = GetSelectedOrAllSectors();
            var hasSectors = sectors.Any();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Sector", WindowHelper.DescribeSectors(sectors));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!hasSectors);

            if (GUILayout.Button(
                new GUIContent(
                    "Spawn",
                    "Creates asteroids inside asteroid clusters of every sector"),
                GuiHelper.ButtonLayout))
            {
                var count = AsteroidSpawnTool.SpawnAsteroidsInSectors(sectors);

                var message = count > 0 ?
                    $"Finished creating {count} asteroids" :
                    "No asteroids were created. Ensure the selected sectors have asteroid clusters or aren't already filled with asteroids";

                EditorUtility.DisplayDialog("Spawn asteroids", message, "OK");
            }

            EditorGUI.EndDisabledGroup();
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
            var prefabs = GameObjectHelper.GetPrefabsOfTypeFromPath<EditorUnit>(settings.UnitPrefabsPath.Trim('/') + "/" + subDirectory).ToList();

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

                    unit.transform.position = SpawnWindowHelper.GetNewUnitSpawnPosition(sector, unit.GetCollisionRadius());

                    Selection.objects = new GameObject[] { unit.gameObject };

                    // Auto-frame on spawned object
                    //var viewSize = 100.0f;;
                    //SceneView.lastActiveSceneView.Frame(new Bounds(unit.transform.position, new Vector3(viewSize, viewSize, viewSize)), false);
                }
            }
        }
    }
}
