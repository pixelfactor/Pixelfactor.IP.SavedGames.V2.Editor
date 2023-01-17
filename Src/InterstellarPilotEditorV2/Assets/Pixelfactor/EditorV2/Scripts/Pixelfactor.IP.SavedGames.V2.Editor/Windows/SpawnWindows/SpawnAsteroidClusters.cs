﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.AsteroidClusters;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows
{
    public class SpawnAsteroidClusters
    {
        public void Draw()
        {
            DrawAutoSpawnOptions(null);

            GuiHelper.Subtitle("Spawn single asteroid clusters", "Create asteroid clusters in selected sectors");

            EditorFaction editorFaction = null;
            SpawnWindowHelper.ShowSpawnUnitOptions("AsteroidCluster", allowFaction: false, ref editorFaction);

            DrawDeleteOptions();
        }

        private void DrawAutoSpawnOptions(IEnumerable<EditorUnit> asteroidClusterPrefabs)
        {
            GuiHelper.Subtitle("Auto-spawn clusters", "Spawn asteroid clusters in all sectors based on settings");

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
                var sectorsToSpawnAsteroidClusters = AsteroidClusterSpawnTool.GetNewAsteroidClusterSectors(sectors, settings);

                // Find all existing planets - use this to try and spawn unique ones
                var allAsteroidClusters = SavedGameUtil.FindSavedGameOrErrorOut().GetComponentsInChildren<EditorAsteroidCluster>().Select(e => e.GetComponent<EditorUnit>()).ToList();
                var count = 0;
                foreach (var sector in sectorsToSpawnAsteroidClusters)
                {
                    Debug.Log($"Spawning asteroid cluster in {sector.Name}");
                    //var newPlanet = SpawnNewAsteroidCluster(sector, autoPositionPlanet: true, planetPrefabs, settings, allAsteroidClusters);
                    //if (newPlanet != null)
                    //{
                    //    allAsteroidClusters.Add(newPlanet);
                    //    count++;
                    //}
                }

                //var count = PlanetSpawnTool.SpawnPlanetsInSectors(sectors, );

                var message = count > 0 ?
                    $"Finished creating {count} asteroid clusters" :
                    "No asteroid clusters were created. Ensure that asteroid cluster prefabs can be found";

                EditorUtility.DisplayDialog("Spawn planets", message, "OK");
            }

            EditorGUI.EndDisabledGroup();
        }

        private EditorUnit SpawnNewAsteroidCluster(EditorUnit prefab, EditorSector sector, bool autoPosition, CustomSettings settings)
        {
            var planetUnit = PrefabHelper.Instantiate(prefab, sector.transform);

            if (autoPosition)
            {
                //AutoPositionAsteroidClusterTool.AutoPositionPlanet(planetUnit, sector, settings);
            }
            else
            {
                planetUnit.transform.position = SpawnWindowHelper.GetNewUnitSpawnPosition(sector, 1000.0f);
            }

            AutoNameObjects.AutoNameUnit(planetUnit);

            return planetUnit;
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
