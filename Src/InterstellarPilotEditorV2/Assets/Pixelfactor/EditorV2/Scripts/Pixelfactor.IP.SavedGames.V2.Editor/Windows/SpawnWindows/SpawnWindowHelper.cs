using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows
{
    public static class SpawnWindowHelper
    {
        public static Vector3 GetNewUnitSpawnPosition(EditorSector sector, float radius)
        {
            var initialPosition = GetCurrentScenePositionInScene();

            var newPosition = SpawnPositionFinder.FindPositionOrNull(sector, initialPosition, radius);
            if (newPosition.HasValue)
            {
                return newPosition.Value;
            }

            return sector.transform.position;
        }

        public static Vector3 GetSceneViewCenter()
        {
            return GetCurrentScenePositionInScene(SceneView.lastActiveSceneView.position.center);
        }

        public static Vector3 GetCurrentScenePositionInScene()
        {
            return GetCurrentScenePositionInScene(Event.current.mousePosition);
        }

        public static Vector3 GetCurrentScenePositionInScene(Vector2 screenSpace)
        {
            Vector3 mousePosition = screenSpace;
            var placeObject = HandleUtility.PlaceObject(mousePosition, out var newPosition, out var normal);
            var p = placeObject ? newPosition : HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10);
            p.y = 0.0f;
            return p;
        }

        public static void ShowSpawnUnitOptions(string subDirectory, bool allowFaction, ref EditorFaction spawnFaction)
        {
            var sector = Selector.GetSingleSelectedSectorOrNull();

            var canSpawn = sector != null;

            EditorGUI.BeginDisabledGroup(!canSpawn);

            if (allowFaction)
            {
                var factionContent = new GUIContent("Spawn faction", "The faction that the spawned unit will be assigned to");
                spawnFaction = (EditorFaction)EditorGUILayout.ObjectField(factionContent, spawnFaction, typeof(EditorFaction), allowSceneObjects: true);
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

                    DrawSpawnPrefabButton(sector, unitPrefab, spawnFaction);

                    i++;
                }

                if (columnCount > 1)
                {
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUI.EndDisabledGroup();
        }

        private static void DrawSpawnPrefabButton(EditorSector sector, EditorUnit unitPrefab, EditorFaction editorFaction)
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

        public static List<EditorSector> GetSelectedOrAllSectors()
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
    }
}
