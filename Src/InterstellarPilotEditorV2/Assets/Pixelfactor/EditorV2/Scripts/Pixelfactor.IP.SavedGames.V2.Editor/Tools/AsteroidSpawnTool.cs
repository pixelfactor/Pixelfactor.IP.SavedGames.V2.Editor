using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public class AsteroidSpawnTool
    {
        public static int SpawnAsteroids()
        {
            var editorScenario = SavedGameUtil.FindSavedGameOrErrorOut();

            var createdCount = new AsteroidSpawner().SpawnInAllClusters(editorScenario);

            Debug.Log($"Finished creating {createdCount} asteroids");

            return createdCount;
        }

        public static int SpawnAsteroidsInSectors(IEnumerable<EditorSector> sectors)
        {
            var createdCount = 0;

            var spawner = new AsteroidSpawner();
            foreach (var sector in sectors)
            {
                createdCount += spawner.SpawnInSector(sector);
            }

            Debug.Log($"Finished creating {createdCount} asteroids");

            return createdCount;
        }
    }
}
