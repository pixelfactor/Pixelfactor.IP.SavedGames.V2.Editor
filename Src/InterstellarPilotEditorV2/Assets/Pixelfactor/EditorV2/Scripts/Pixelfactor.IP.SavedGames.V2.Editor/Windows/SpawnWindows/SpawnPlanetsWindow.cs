using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Planets;
using Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows;
using Pixelfactor.IP.SavedGames.V2.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows
{
    public class SpawnPlanetsWindow
    {
        private bool autoPositionPlanets = true;

        public void Draw()
        {
            var settings = CustomSettings.GetOrCreateSettings();
            var allPlanetPrefabs = GetAllPlanetPrefabs(settings);

            var planetPrefabs = GetPlanetPrefabs(allPlanetPrefabs);
            var moonPrefabs = GetMoonPrefabs(allPlanetPrefabs);

            DrawPlanetAutoSpawnOptions(settings);

            DrawSpawnPlanetOptions(allPlanetPrefabs, planetPrefabs, moonPrefabs);

            DrawDeletePlanetOptions();
        }

        public IEnumerable<EditorUnit> GetAllPlanetPrefabs(CustomSettings settings)
        {
            var planetPrefabPath = settings.UnitPrefabsPath.TrimEnd('/') + "/" + "Planet";
            var allPlanetPrefabs = GameObjectHelper.GetPrefabsOfTypeFromPath<EditorUnit>(planetPrefabPath).Where(e => e.ModelUnitClass != ModelUnitClass.Planet_Earth).ToList();

            return allPlanetPrefabs;
        }

        public IEnumerable<EditorUnit> GetPlanetPrefabs(CustomSettings settings)
        {
            return GetPlanetPrefabs(GetAllPlanetPrefabs(settings));
        }

        public IEnumerable<EditorUnit> GetPlanetPrefabs(IEnumerable<EditorUnit> allPlanetPrefabs)
        {
            return allPlanetPrefabs.Where(e => !e.name.Contains("moon", System.StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public IEnumerable<EditorUnit> GetMoonPrefabs(IEnumerable<EditorUnit> allPlanetPrefabs)
        {
            return allPlanetPrefabs.Where(e => e.name.Contains("moon", System.StringComparison.InvariantCultureIgnoreCase)).ToList();
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

        private void DrawPlanetAutoSpawnOptions(CustomSettings settings)
        {
            GuiHelper.Subtitle("Auto-spawn planets", "Spawn planets in all sectors based on settings");

            var sectors = SpawnWindowHelper.GetAllSectors();
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
                var sectorsToSpawnPlanets = PlanetSpawnTool.GetNewPlanetSectors(sectors, settings);

                var count = SpawnPlanetsInSectors(
                    sectorsToSpawnPlanets,
                    settings);

                var message = count > 0 ?
                    $"Finished creating {count} planets" :
                    "No planets were created. Ensure that planet prefabs can be found";

                EditorUtility.DisplayDialog("Spawn planets", message, "OK");
            }

            EditorGUI.EndDisabledGroup();
        }

        public int SpawnPlanetsInSectors(
            IEnumerable<EditorSector> sectorsToSpawnPlanets,
            CustomSettings settings)
        {
            var planetPrefabs = GetPlanetPrefabs(settings);

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

            return count;
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
    }
}
