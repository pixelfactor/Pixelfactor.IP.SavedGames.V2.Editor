﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
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
        private int currentTab = SpawnShipId;
        private EditorFaction spawnFaction = null;

        public void Draw()
        {

            currentTab = GUILayout.Toolbar(currentTab, new string[] { "Ship", "Station", "Asteroids" });

            switch (currentTab)
            {
                case SpawnShipId:
                    {
                        ShowSpawnOptions("Ship");
                    }
                    break;
                case SpawnStationId:
                    {
                        ShowSpawnOptions("Station");
                    }
                    break;
                case SpawnAsteroidsId:
                    {
                        DrawSpawnAsteroidOptions();

                        EditorGUILayout.Space(30);

                        ShowSpawnOptions("Asteroid", allowFaction: false);
                    }
                    break;
            }

            EditorGUI.EndDisabledGroup();
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

        private void ShowSpawnOptions(string subDirectory, bool allowFaction = true)
        {
            var sector = Selector.GetSingleSelectedSectorOrNull();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Spawn Sector", WindowHelper.DescribeSectors(sector));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(true);


            EditorGUI.EndDisabledGroup();

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
                    var radius = 1.0f;
                    var sphereCollider = unit.GetComponentInChildren<SphereCollider>();
                    if (sphereCollider != null)
                        radius = sphereCollider.radius;

                    if (unit.CanHaveFaction())
                    {
                        unit.Faction = editorFaction;
                    }

                    var initialPosition = GetCurrentScenePositionInScene();
                    var newPosition = SpawnPositionFinder.FindPositionOrNull(sector, initialPosition, radius);
                    if (newPosition.HasValue)
                    {
                        unit.transform.position = newPosition.Value;
                    }
                    else
                    {
                        unit.transform.localPosition = Vector3.zero;
                    }

                    Selection.objects = new GameObject[] { unit.gameObject };

                    // Auto-frame on spawned object
                    //var viewSize = 100.0f;;
                    //SceneView.lastActiveSceneView.Frame(new Bounds(unit.transform.position, new Vector3(viewSize, viewSize, viewSize)), false);
                }
            }
        }
    }
}
