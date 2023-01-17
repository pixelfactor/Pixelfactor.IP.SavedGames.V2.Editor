using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    /// <summary>
    /// Window that allows the user to create a new scenario
    /// </summary>
    public class CreateWindow : EditorWindow
    {
        private int numSectors = 8;

        private bool createPlanets = true;

        private bool createAsteroidClusters = true;

        private bool createClusterGasClouds = true;

        private bool createAsteroids = true;

        void OnGUI()
        {
            GuiHelper.Subtitle("Create custom scenario", "Creates a new scenario based on paramters. It is recommended to create smaller universes!");

            EditorGUILayout.PrefixLabel(new GUIContent("Sectors", "The number of sectors that the universe should have"));
            this.numSectors = EditorGUILayout.IntSlider(this.numSectors, 1, 256, GUILayout.ExpandWidth(false));

            EditorGUILayout.Space();

            this.createPlanets = EditorGUILayout.Toggle(new GUIContent("Create planets", "Whether to create planets"), this.createPlanets, GUILayout.ExpandWidth(false));
            this.createAsteroidClusters = EditorGUILayout.Toggle(new GUIContent("Create asteroid clusters", "Whether to create asteroid clusters"), this.createAsteroidClusters, GUILayout.ExpandWidth(false));

            var canCreateClusterGasClouds = this.createAsteroidClusters;
            if (!canCreateClusterGasClouds)
            {
                this.createClusterGasClouds = false;
            }

            EditorGUI.BeginDisabledGroup(!canCreateClusterGasClouds);
            this.createClusterGasClouds = EditorGUILayout.Toggle(new GUIContent("Create cluster gas clouds", "Whether to create gas clouds around asteroid clusters"), this.createClusterGasClouds, GUILayout.ExpandWidth(false));
            EditorGUI.EndDisabledGroup();

            var canCreateAsteroids = this.createAsteroidClusters;
            if (!canCreateAsteroids)
            {
                this.createAsteroids = false;
            }

            EditorGUI.BeginDisabledGroup(!canCreateAsteroids);
            this.createAsteroids = EditorGUILayout.Toggle(new GUIContent("Create asteroids", "Whether to create asteroids inside asteroid clusters"), this.createAsteroids, GUILayout.ExpandWidth(false));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            if (GUILayout.Button(new GUIContent(
                "Create",
                "Creates the universe (this may take a while for a biggun)"),
                GuiHelper.ButtonLayout))
            {
                OnCreate();
            }
        }

        private void OnCreate()
        {
            Debug.Log($"Creating a new scenario with {this.numSectors} sectors...");

            var scenario = CreateNewScenarioTool.CreateNewSingleSector();

            var firstSector = scenario.GetSectors().FirstOrDefault();

            if (firstSector == null)
            {
                Debug.LogError("Expected that there would be one sector created", this);
                return;
            }

            var settings = CustomSettings.GetOrCreateSettings();
            var newSectorsToCreate = this.numSectors - 1;
            if (newSectorsToCreate > 0)
            {
                var sectors = new EditorSector[] { firstSector }.ToList();
                new BuildWindow().AutoGrow(sectors, sectors, settings, newSectorsToCreate);
            }

            var allSectors = scenario.GetSectors();

            if (this.createPlanets)
            {
                var sectorsToSpawnPlanets = PlanetSpawnTool.GetNewPlanetSectors(allSectors.ToList(), settings);
                new SpawnPlanetsWindow().SpawnPlanetsInSectors(sectorsToSpawnPlanets, settings);
            }

            if (this.createAsteroidClusters)
            {
                new SpawnAsteroidClustersWindow().AutoSpawnAsteroidClustersInSectors(allSectors, settings);
            }

            if (this.createAsteroids)
            {
                AsteroidSpawnTool.SpawnInClustersInSectors(allSectors);
            }

            Debug.Log($"Finished creating new custom scenario");

            GUIUtility.ExitGUI();
        }
        public static void ShowNew()
        {
            var window = EditorWindow.GetWindow<CreateWindow>();
            var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Pixelfactor/EditorV2/Textures/DevLogoCentered256.png");
            window.titleContent = new GUIContent("Create", icon);
        }
    }
}
