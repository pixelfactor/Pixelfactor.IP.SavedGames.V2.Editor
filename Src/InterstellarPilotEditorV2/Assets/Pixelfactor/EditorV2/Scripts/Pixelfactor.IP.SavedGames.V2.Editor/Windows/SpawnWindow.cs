using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
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
        public void Draw()
        {
            var sectors = Selector.GetInParents<EditorSector>();

            var hasSectors = sectors.Any();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Sectors", WindowHelper.DescribeSectors(sectors));
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

            if (GUILayout.Button(
                new GUIContent(
                    "Spawn Shuttle A",
                    "Creates a Shuttle A ship"),
                GuiHelper.ButtonLayout))
            {
                var count = AsteroidSpawnTool.SpawnAsteroidsInSectors(sectors);
                var sector = Selector.GetSingleSelectedSectorOrNull();

                if (sector == null)
                {
                    EditorUtility.DisplayDialog("Spawn", "Select a sector first", "OK");
                }
                else
                {
                    var unit = Spawn.Unit(sector, Model.ModelUnitClass.Ship_ShuttleA, CustomSettings.GetOrCreateSettings().UnitPrefabsPath);
                    var radius = 1.0f;
                    var sphereCollider = unit.GetComponentInChildren<SphereCollider>();
                    if (sphereCollider != null)
                        radius = sphereCollider.radius;

                    var newPosition = SpawnPositionFinder.FindPositionOrNull(sector, sector.transform.position, radius);
                    if (newPosition.HasValue)
                    {
                        unit.transform.position = newPosition.Value;
                    }
                    else
                    {
                        unit.transform.localPosition = Vector3.zero;
                    }

                    Selection.objects = new GameObject[] { unit.gameObject };
                    SceneView.lastActiveSceneView.Frame(new Bounds(unit.transform.position, new Vector3(10.0f, 10.0f, 10.0f)), true);
                }
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
