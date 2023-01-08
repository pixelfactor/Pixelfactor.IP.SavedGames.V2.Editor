﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Utilities.Spawning
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

        public string AsteroidTypeBPrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/Units/Asteroids/Unit_AsteroidTypeB_Rock.prefab";
        public string AsteroidTypeHPrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/Units/Asteroids/Unit_AsteroidTypeH_Ice.prefab";

        public void CreateAsteroids()
        {
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();

            foreach (var asteroidCluster in editorSavedGame.GetComponentsInChildren<EditorAsteroidCluster>())
            {
                PopulateAsteroidsAroundCluster(asteroidCluster);
            }
        }

        private EditorUnit GetAsteroidPrefab(EditorAsteroidCluster asteroidCluster)
        {
            switch (asteroidCluster.GetComponent<EditorUnit>().ModelUnitClass)
            {
                case Model.ModelUnitClass.AsteroidCluster_TypeB:
                    {
                        var unit = AssetDatabase.LoadAssetAtPath<EditorUnit>(this.AsteroidTypeBPrefabPath);
                        if (unit == null)
                        {

                        }

                        return unit;
                    }
                case Model.ModelUnitClass.AsteroidCluster_TypeH:
                    {
                        var unit = AssetDatabase.LoadAssetAtPath<EditorUnit>(this.AsteroidTypeHPrefabPath);
                        if (unit == null)
                        {
                            throw new System.Exception($"Cannot create asteroids in asteroid cluster {asteroidCluster}. No asteroid prefab for asteroid cluster found at path \"{AsteroidTypeHPrefabPath}\"");
                        }

                        return unit;
                    }
            }

            throw new System.Exception($"Cannot create asteroids in asteroid cluster {asteroidCluster}. No asteroid prefab defined for this asteroid cluster");
        }

        public void PopulateAsteroidsAroundCluster(EditorAsteroidCluster asteroidCluster)
        {
            var asteroidPrefab = GetAsteroidPrefab(asteroidCluster); ;

            if (asteroidPrefab != null)
            {
                PopulateAsteroidsAroundCluster(asteroidCluster, asteroidPrefab);
            }
            else
            {
                throw new System.Exception($"Cannot create asteroids in asteroid cluster {asteroidCluster}. No asteroid prefabs for asteroid cluster found");
            }
        }

        public void PopulateAsteroidsAroundCluster(EditorAsteroidCluster asteroidCluster, EditorUnit asteroidPrefab)
        {
            var sector = asteroidCluster.GetComponentInParent<EditorSector>();

            if (sector == null) 
                throw new System.Exception("Asteroid cluster should have a parent sector");

            var min = GetMinDesiredAsteroids(asteroidCluster);
            var max = GetMaxDesiredAsteroids(asteroidCluster);

            var count = Random.Range(min, max + 1);

            for (int i = 0; i < count; i++)
            {
                var asteroid = TryGenerateAsteroid(asteroidCluster, sector, asteroidPrefab);
            }
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

        public EditorUnit TryGenerateAsteroid(EditorAsteroidCluster asteroidCluster, EditorSector sector, EditorUnit asteroidPrefab)
        {
            var position = TryGenerateAsteroidSectorPosition(asteroidCluster, sector);
            if (position.HasValue)
            {
                var asteroid = CreateAsteroid(asteroidPrefab, sector, position.Value);
                return asteroid;
            }

            return null;
        }

        public EditorUnit CreateAsteroid(EditorUnit prefab, EditorSector sector, Vector3 position)
        {
            var asteroid = GameObject.Instantiate(prefab);
            asteroid.transform.SetParent(sector.transform, true);
            asteroid.transform.position = position;
            asteroid.transform.localRotation = Random.rotationUniform;
            return asteroid;
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
