using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning
{
    public class AsteroidSpawner
    {
        public int MinAsteroidCount = 3;

        public int MaxAsteroidCount = 5;

        public float MinDistanceBetweenAsteroids = 175.0f;

        /// <summary>
        /// Determines at what radius of asteroid cluster the min and max count apply
        /// </summary>
        public float AsteroidCountReferenceRadius = 1000.0f;

        /// <summary>
        /// Create asteroids in all sectors
        /// </summary>
        public int SpawnInAllClusters(EditorScenario editorScenario)
        {
            var count = 0;
            foreach (var asteroidCluster in editorScenario.GetComponentsInChildren<EditorAsteroidCluster>())
            {
                count += SpawnAsteroidsAroundCluster(asteroidCluster);
            }

            return count;
        }

        /// <summary>
        /// Create asteroids in all sectors
        /// </summary>
        public int SpawnInSector(EditorSector editorSector)
        {
            var count = 0;
            foreach (var asteroidCluster in editorSector.GetComponentsInChildren<EditorAsteroidCluster>())
            {
                count += SpawnAsteroidsAroundCluster(asteroidCluster);
            }

            return count;
        }

        private EditorUnit GetAsteroidPrefab(EditorAsteroidCluster asteroidCluster)
        {
            var settings = CustomSettings.GetOrCreateSettings();

            switch (asteroidCluster.GetComponent<EditorUnit>().ModelUnitClass)
            {
                case Model.ModelUnitClass.AsteroidCluster_TypeB:
                    {
                        var unit = AssetDatabase.LoadAssetAtPath<EditorUnit>(Spawn.GetUnitPrefabPath(settings.UnitPrefabsPath, Model.ModelUnitClass.Asteroid_TypeB));
                        if (unit == null)
                        {
                            throw new System.Exception($"Cannot create asteroids in asteroid cluster {asteroidCluster}. No asteroid prefab for asteroid cluster found in path \"{settings.UnitPrefabsPath}\"");
                        }

                        return unit;
                    }
                case Model.ModelUnitClass.AsteroidCluster_TypeH:
                    {
                        var unit = AssetDatabase.LoadAssetAtPath<EditorUnit>(Spawn.GetUnitPrefabPath(settings.UnitPrefabsPath, Model.ModelUnitClass.Asteroid_TypeH));
                        if (unit == null)
                        {
                            throw new System.Exception($"Cannot create asteroids in asteroid cluster {asteroidCluster}. No asteroid prefab for asteroid cluster found in path \"{settings.UnitPrefabsPath}\"");
                        }

                        return unit;
                    }
            }

            throw new System.Exception($"Cannot create asteroids in asteroid cluster {asteroidCluster}. No asteroid prefab defined for this asteroid cluster");
        }

        public int SpawnAsteroidsAroundCluster(EditorAsteroidCluster asteroidCluster)
        {
            var asteroidPrefab = GetAsteroidPrefab(asteroidCluster); ;

            if (asteroidPrefab != null)
            {
                return SpawnAsteroidsAroundCluster(asteroidCluster, asteroidPrefab);
            }

            throw new System.Exception($"Cannot create asteroids in asteroid cluster {asteroidCluster}. No asteroid prefabs for asteroid cluster found");
        }

        public int SpawnAsteroidsAroundCluster(EditorAsteroidCluster asteroidCluster, EditorUnit asteroidPrefab)
        {
            var sector = asteroidCluster.GetComponentInParent<EditorSector>();

            if (sector == null) 
                throw new System.Exception("Asteroid cluster should have a parent sector");

            var min = GetMinDesiredAsteroids(asteroidCluster);
            var max = GetMaxDesiredAsteroids(asteroidCluster);

            var count = Random.Range(min, max + 1);

            var actualCreatedCount = 0;

            for (int i = 0; i < count; i++)
            {
                var asteroid = TrySpawnAsteroid(asteroidCluster, sector, asteroidPrefab);
                if (asteroid != null)
                {
                    actualCreatedCount++;
                }
            }

            return actualCreatedCount;
        }

        public int GetMinDesiredAsteroids(EditorAsteroidCluster asteroidCluster)
        {
            return Mathf.CeilToInt(
                MinAsteroidCount * (asteroidCluster.GetComponent<EditorUnit>().Radius / AsteroidCountReferenceRadius));
        }

        public int GetMaxDesiredAsteroids(EditorAsteroidCluster asteroidCluster)
        {
            return Mathf.CeilToInt(
                MaxAsteroidCount * (asteroidCluster.GetComponent<EditorUnit>().Radius / AsteroidCountReferenceRadius));
        }

        public EditorUnit TrySpawnAsteroid(EditorAsteroidCluster asteroidCluster, EditorSector sector, EditorUnit asteroidPrefab)
        {
            var position = TryGenerateAsteroidSectorPosition(asteroidCluster, sector);
            if (position.HasValue)
            {
                var asteroid = SpawnAsteroid(asteroidPrefab, sector, position.Value);
                return asteroid;
            }

            return null;
        }

        public EditorUnit SpawnAsteroid(EditorUnit prefab, EditorSector sector, Vector3 position)
        {
            var asteroid = PrefabUtility.InstantiatePrefab(prefab.gameObject, sector.transform) as GameObject;
            asteroid.transform.position = position;
            asteroid.transform.localRotation = Random.rotationUniform;
            return asteroid.GetComponent<EditorUnit>();
        }

        public Vector3? TryGenerateAsteroidSectorPosition(EditorAsteroidCluster asteroidCluster, EditorSector sector)
        {
            var dist = Random.Range(0.0f, asteroidCluster.GetComponent<EditorUnit>().Radius * 0.9f);
            var randomPosition = asteroidCluster.GetComponent<EditorUnit>().transform.position + (Geometry.RandomXZUnitVector() * dist);

            return SpawnPositionFinder.FindPositionOrNull(
                sector,
                randomPosition,
                MinDistanceBetweenAsteroids);
        }
    }
}
