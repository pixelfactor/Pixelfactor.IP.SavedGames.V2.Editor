using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorAsteroidCluster : MonoBehaviour
    {
        /// <summary>
        /// Gas cloud that is associated with this type of asteroid cluster
        /// </summary>
        [Tooltip("The prefab for the gas cloud that this cluster may generate")]
        public EditorUnit GasCloudPrefab;

        [Tooltip("The asteroid unit that this cluster will generate")]
        public EditorAsteroidType AsteroidType;

        [ContextMenu("Spawn asteroids")]
        public void SpawnAsteroids()
        {
            var count = new AsteroidSpawner().SpawnAsteroidsAroundCluster(this);
            Debug.Log($"Spawned {count} asteroids");
        }
    }
}
