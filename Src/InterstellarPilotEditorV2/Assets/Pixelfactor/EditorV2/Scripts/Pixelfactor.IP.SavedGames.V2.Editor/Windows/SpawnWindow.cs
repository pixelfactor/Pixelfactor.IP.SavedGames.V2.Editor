using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
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
            EditorGUILayout.TextField("Sectors", GetSectorLabel(sectors));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!hasSectors);

            if (GUILayout.Button(new GUIContent(
                "Spawn asteroids",
                "Creates asteroids inside asteroid clusters")))
            {
                var count = AsteroidSpawnTool.SpawnAsteroidsInSectors(sectors);

                var message = count > 0 ?
                    $"Finished creating {count} asteroids" : 
                    "No asteroids were created. Ensure the selected sectors have asteroid clusters or aren't already filled with asteroids";

                EditorUtility.DisplayDialog("Spawn asteroids", message, "OK");
            }

            EditorGUI.EndDisabledGroup();
        }

        private string GetSectorLabel(IEnumerable<EditorSector> sectors)
        {
            if (sectors.Count() == 0)
            {
                return "[None]";
            }

            if (sectors.Count() == 1)
            {
                return sectors.First().Name;
            }

            if (sectors.Count() < 4)
            {
                return string.Join(", ", sectors.Select(e => e.Name));
            }

            return $"{sectors.Count()} sectors";
        }
    }
}
