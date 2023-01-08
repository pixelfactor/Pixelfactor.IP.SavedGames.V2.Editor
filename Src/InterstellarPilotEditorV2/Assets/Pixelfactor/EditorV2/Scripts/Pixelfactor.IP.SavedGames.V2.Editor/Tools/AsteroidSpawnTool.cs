using Pixelfactor.IP.SavedGames.V2.Editor.Utilities;
using Pixelfactor.IP.SavedGames.V2.Editor.Utilities.Spawning;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public class AsteroidSpawnTool
    {
        [MenuItem("Window/IP Editor V2/Spawn/Create asteroids in clusters")]
        public static void AutoAssignIdsMenuItem()
        {
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();

            var createdCount = new AsteroidSpawner().CreateAsteroids(editorSavedGame);

            Debug.Log($"Finished creating {createdCount} asteroids");
        }
    }
}
