using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.AsteroidClusters;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows
{
    public class SpawnAsteroidClustersWindow
    {
        public void Draw()
        {
            DrawAutoSpawnOptions();

            GuiHelper.Subtitle("Spawn single asteroid cluster", "Create an asteroid cluster in selected sectors");

            EditorFaction editorFaction = null;
            SpawnWindowHelper.ShowSpawnUnitOptions("AsteroidCluster", allowFaction: false, ref editorFaction);

            DrawDeleteOptions();
        }

        private void DrawAutoSpawnOptions()
        {
            GuiHelper.Subtitle("Auto-spawn asteroid clusters", "Spawn asteroid clusters in all sectors based on settings");

            var sectors = SpawnWindowHelper.GetAllSectors();
            var hasSectors = sectors.Any();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Sector", WindowHelper.DescribeSectors(sectors));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!hasSectors);

            if (GUILayout.Button(
                new GUIContent(
                    "Auto spawn",
                    "Creates asteroid clusters automatically in all sectors"),
                GuiHelper.ButtonLayout))
            {
                var settings = CustomSettings.GetOrCreateSettings();


                var message = "No asteroid clusters were created. Ensure that there are any sectors selected where asteroids can be spawned";

                var spawnedAsteroidClusters = AutoSpawnAsteroidClustersInSectors(sectors, settings);
                var count = spawnedAsteroidClusters.Count;

                if (count > 0)
                {
                    var spawnedAsteroidClustersByType = spawnedAsteroidClusters.Select(e => e.GetComponent<EditorAsteroidCluster>())
                        .GroupBy(e => e.AsteroidType)
                        .Where(e => e.Count() > 0);

                    var statusText = string.Join(", ", spawnedAsteroidClustersByType.Select(grouping => $"{grouping.Count()} x {grouping.First().AsteroidType.Name}"));
                    message = $"Finished creating {statusText}";
                }


                EditorUtility.DisplayDialog("Spawn planets", message, "OK");
            }

            EditorGUI.EndDisabledGroup();
        }

        public List<EditorUnit> AutoSpawnAsteroidClustersInSectors(IEnumerable<EditorSector> sectors, CustomSettings settings)
        {
            var asteroidTypes = PrefabHelper.GetAsteroidTypes(settings);

            if (asteroidTypes.Count == 0)
            {
                Debug.LogError("No asteroid types were found. Check that path to asteroid type prefabs is valid in settings");
                return new List<EditorUnit>();
            }

            return AutoSpawnAsteroidClustersInSectors(sectors, asteroidTypes, settings);
        }

        public List<EditorUnit> AutoSpawnAsteroidClustersInSectors(IEnumerable<EditorSector> sectors, List<EditorAsteroidType> asteroidTypes, CustomSettings settings)
        {
            var selectedSectors = sectors.ToList();

            var sectorsToSpawnAsteroidClusters = AsteroidClusterSpawnTool.GetNewAsteroidClusterSectors(selectedSectors, settings);

            // Find all existing asteroid clusters - use this to try and spawn unique ones
            var allAsteroidClusters = SavedGameUtil.FindSavedGameOrErrorOut().GetComponentsInChildren<EditorAsteroidCluster>().Select(e => e.GetComponent<EditorUnit>()).ToList();
            var count = 0;

            var allSectors = SpawnWindowHelper.GetAllSectors();

            var spawnedAsteroidClusters = new List<EditorUnit>();
            foreach (var sector in sectorsToSpawnAsteroidClusters)
            {
                Debug.Log($"Spawning asteroid cluster(s) in {sector.Name}");

                foreach (var spawnedAsteroidCluster in AsteroidClusterSpawnTool.CreateSectorAsteroidClusters(sector, allSectors, asteroidTypes, settings))
                {
                    Debug.Log($"Spawned asteroid cluster in {sector.Name}");

                    spawnedAsteroidClusters.Add(spawnedAsteroidCluster);
                    allAsteroidClusters.Add(spawnedAsteroidCluster);
                    count++;
                }
            }

            return spawnedAsteroidClusters;
        }

        private void DrawDeleteOptions()
        {
            GuiHelper.Subtitle("Delete clusters", "Deletes asteroid clusters from the selected sectors");
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
                    $"Delete asteroid clusters",
                    $"Deletes asteroid clusters from the selected sectors"),
                GuiHelper.ButtonLayout))
            {
                foreach (var sector in selectedSectors)
                {
                    foreach (var unit in sector.GetComponentsInChildren<EditorUnit>())
                    {
                        if (unit.IsAsteroidCluster())
                        {
                            GameObject.DestroyImmediate(unit.gameObject);
                        }
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
