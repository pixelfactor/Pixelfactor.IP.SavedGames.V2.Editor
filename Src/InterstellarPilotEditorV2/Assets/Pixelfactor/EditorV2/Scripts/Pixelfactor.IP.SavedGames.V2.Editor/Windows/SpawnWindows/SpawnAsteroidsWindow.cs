using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows
{
    public class SpawnAsteroidsWindow
    {
        public void Draw()
        {
            DrawAsteroidAutoSpawnOptions();

            EditorGUILayout.Space(30);

            GuiHelper.Subtitle("Spawn single asteroid", "Spawn an asteroid in a specific sector");
            EditorFaction spawnFaction = null;
            SpawnWindow.ShowSpawnOptionsAndSector("Asteroid", allowFaction: false, ref spawnFaction);
        }

        private static void DrawAsteroidAutoSpawnOptions()
        {
            GuiHelper.Subtitle("Auto-spawn asteroids", "Spawn asteroids in selected sectors based on existing asteroid clusters");
            var sectors = SpawnWindowHelper.GetSelectedOrAllSectors();
            var hasSectors = sectors.Any();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Sector", WindowHelper.DescribeSectors(sectors));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!hasSectors);

            if (GUILayout.Button(
                new GUIContent(
                    "Auto spawn",
                    "Creates asteroids inside asteroid clusters of every sector"),
                GuiHelper.ButtonLayout))
            {
                var count = AsteroidSpawnTool.SpawnInClustersInSectors(sectors);

                var message = count > 0 ?
                    $"Finished creating {count} asteroids" :
                    "No asteroids were created. Ensure the selected sectors have asteroid clusters or aren't already filled with asteroids";

                EditorUtility.DisplayDialog("Spawn asteroids", message, "OK");
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
