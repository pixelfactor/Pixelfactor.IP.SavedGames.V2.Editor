using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.AsteroidClusters
{
    public static class AsteroidClusterSpawnTool
    {
        public static List<EditorSector> GetNewAsteroidClusterSectors(List<EditorSector> sectors, CustomSettings settings)
        {
            var newAsteroidSectors = new List<EditorSector>();
            var planetSectors = new List<EditorSector>();
            var asteroidClusterSectors = new List<EditorSector>();

            foreach (var sector in sectors)
            {
                if (sector.HasPlanets())
                {
                    planetSectors.Add(sector);
                }

                if (sector.HasAsteroidClusters())
                {
                    asteroidClusterSectors.Add(sector);
                }
            }

            var totalWeighting = settings.PlanetSectorWeighting + settings.DeepSpaceSectorWeighting + settings.AsteroidSectorWeighting;

            var numAsteroidSectorsToCreate = Mathf.Max(
                settings.MinNumberAsteroidSectors,
                Mathf.RoundToInt(settings.AsteroidSectorWeighting / totalWeighting * sectors.Count));

            // Cap to the number of scenes available
            var numExistingAsteroidSectors = asteroidClusterSectors.Count;

            var numPlanetSectors = planetSectors.Count;
            numAsteroidSectorsToCreate = Mathf.Min(numAsteroidSectorsToCreate, sectors.Count - numPlanetSectors);

            var random = new System.Random();

            for (int i = 0; i < numAsteroidSectorsToCreate - numExistingAsteroidSectors; i++)
            {
                var sectorForAsteroid = GetBestSectorForAsteroid(sectors, planetSectors, asteroidClusterSectors, settings, random);
                if (sectorForAsteroid != null)
                {
                    newAsteroidSectors.Add(sectorForAsteroid);
                }
            }

            return newAsteroidSectors;
        }

        public static EditorSector GetBestSectorForAsteroid(
            IEnumerable<EditorSector> sectors,
            IEnumerable<EditorSector> planetSectors,
            IEnumerable<EditorSector> asteroidClusterSectors,
            CustomSettings settings,
            System.Random random)
        {
            return sectors
                .Where(e => !asteroidClusterSectors.Contains(e))
                .OrderByDescending(e => ScoreSectorForAsteroid(sectors, planetSectors, e, random))
                .FirstOrDefault();
        }

        private static float ScoreSectorForAsteroid(
            IEnumerable<EditorSector> sectors, 
            IEnumerable<EditorSector> planetSectors,
            EditorSector e, 
            System.Random random)
        {
            var score = 0.0f;

            var minDist = PlanetSpawnTool.GetSectorMinDistanceFromPlanetSector(sectors, planetSectors, e);
            if (minDist.HasValue && minDist < 4)
            {
                score += minDist.Value;
            }

            score += random.NextFloat() * 30.0f;

            return score;
        }

        public static IEnumerable<EditorUnit> CreateSectorAsteroidClusters(
            EditorSector sector,
            IEnumerable<EditorSector> allSectors,
            IEnumerable<EditorAsteroidType> asteroidTypes,
            CustomSettings settings)
        {
            var asteroidType = GetAsteroidType(allSectors, asteroidTypes, settings);

            var asteroidClusterPrefab = GetAsteroidClusterPrefab(asteroidType);

            if (asteroidClusterPrefab != null)
            {
                return CreateSectorAsteroidClusters(sector, asteroidClusterPrefab, settings);
            }

            return new EditorUnit[0];
        }

        private static EditorUnit GetAsteroidClusterPrefab(EditorAsteroidType asteroidType)
        {
            if (asteroidType != null)
            {
                return asteroidType.AsteroidClusterPrefab;
            }


            return null;
        }

        private static EditorAsteroidType GetAsteroidType(
            IEnumerable<EditorSector> sectors,
            IEnumerable<EditorAsteroidType> asteroidTypes,
            CustomSettings settings)
        {
            // Find what asteroid types haven't already been populated
            // Use these as a priority to ensure that there is a supply of more different cargo types
            var missingAsteroidTypes = GetMissingAsteroidTypes(sectors, asteroidTypes, settings);

            var asteroidType = missingAsteroidTypes.GetRandom();

            if (asteroidType == null)
            {
                asteroidType = GetRandomAsteroidTypeForSector(sectors, asteroidTypes, settings);
            }

            return asteroidType;
        }

        private static EditorAsteroidType GetRandomAsteroidTypeForSector(
            IEnumerable<EditorSector> sectors,
            IEnumerable<EditorAsteroidType> asteroidTypes,
            CustomSettings settings)
        {
            EditorAsteroidType bestAsteroidType = null;
            var lowestDelta = float.MaxValue;

            var numAsteroidSectors = GetNumAsteroidSectors(sectors);
            var totalWeighting = asteroidTypes.Sum(e => e.PreferredWeighting);

            // Go through all asteroid types. 
            // Work out how many sectors of this type there are
            // Return asteroid type with lowest proportion compared to preferred weight
            foreach (var asteroidType in asteroidTypes)
            {
                var currentSectorCount = GetSectorCountOfAsteroidType(sectors, asteroidType);
                var currentPercentage = (float)currentSectorCount / numAsteroidSectors + (Random.value * settings.AsteroidTypeWeightRandomness);
                var delta = currentPercentage - (asteroidType.PreferredWeighting / totalWeighting);

                if (bestAsteroidType == null || delta < lowestDelta)
                {
                    lowestDelta = delta;
                    bestAsteroidType = asteroidType;
                }
            }

            return bestAsteroidType;
        }

        private static int GetNumAsteroidSectors(IEnumerable<EditorSector> sectors)
        {
            return sectors.Count(e => e.HasAsteroidClusters());
        }

        private static IEnumerable<EditorAsteroidType> GetMissingAsteroidTypes(
            IEnumerable<EditorSector> sectors,
            IEnumerable<EditorAsteroidType> asteroidTypes,
            CustomSettings settings)
        {
            foreach (var asteroidType in asteroidTypes)
            {
                var count = GetSectorCountOfAsteroidType(sectors, asteroidType);
                if (count == 0)
                {
                    yield return asteroidType;
                }
            }
        }

        private static int GetSectorCountOfAsteroidType(IEnumerable<EditorSector> sectors, EditorAsteroidType asteroidType)
        {
            var count = 0;
            foreach (var sector in sectors)
            {
                if (DoesSectorHaveAsteroidType(sector, asteroidType))
                {
                    count++;
                }
            }

            return count;
        }

        private static bool DoesSectorHaveAsteroidType(EditorSector sector, EditorAsteroidType asteroidType)
        {
            foreach (var asteroidCluster in sector.GetComponentsInChildren<EditorAsteroidCluster>())
            {
                if (asteroidCluster.AsteroidType == asteroidType)
                    return true;
            }

            return false;
        }

        public static IEnumerable<EditorUnit> CreateSectorAsteroidClusters(
            EditorSector sector, 
            EditorUnit asteroidClusterPrefab,
            CustomSettings settings)
        {
            var count = GetRandomAsteroidClusterCount(settings);

            var existingAsteroidClusters = sector.GetComponentsInChildren<EditorAsteroidCluster>().Select(e => e.GetComponent<EditorUnit>()).ToList();

            var asteroidClusters = new List<EditorUnit>();

            for (int i = 0; i < count; i++)
            {
                var asteroidClusterUnit = TryGenerateAsteroidCluster(sector, asteroidClusterPrefab, existingAsteroidClusters, settings);
                if (asteroidClusterUnit != null)
                {

                    AutoNameObjects.AutoNameUnit(asteroidClusterUnit);

                    asteroidClusters.Add(asteroidClusterUnit);

                    var asteroidClusterData = asteroidClusterUnit.GetComponent<EditorAsteroidCluster>();

                    if (asteroidClusterData != null && asteroidClusterData.GasCloudPrefab != null && Random.value < settings.ProbabilityOfGeneratingGasCloud)
                    {
                        var gasCloud = CreateGasCloud(sector, asteroidClusterUnit.Radius, asteroidClusterUnit.transform.localPosition, asteroidClusterData.GasCloudPrefab);
                        if (gasCloud != null)
                        {
                            AutoNameObjects.AutoNameUnit(gasCloud);
                        }
                    }
                }
            }

            return asteroidClusters;
        }

        public static EditorUnit CreateGasCloud(EditorSector sector, float radius, Vector3 position, EditorUnit randomGasCloudPrefab)
        {
            var gasCloudUnit = PrefabHelper.Instantiate(randomGasCloudPrefab, sector.transform);
            gasCloudUnit.transform.position = position;
            gasCloudUnit.Radius = radius;

            return gasCloudUnit;
        }

        private static int GetRandomAsteroidClusterCount(CustomSettings settings)
        {
            return Random.Range(settings.MinAsteroidClusterCount, settings.MaxAsteroidClusterCount + 1);
        }

        private static EditorUnit TryGenerateAsteroidCluster(
            EditorSector sector, 
            EditorUnit asteroidClusterPrefab,
            IEnumerable<EditorUnit> existingAsteroidClusters,
            CustomSettings settings)
        {
            var radius = Helper.RandomFloatWithPower(
                settings.MinAsteroidClusterRadius,
                settings.MaxAsteroidClusterRadius,
                settings.AsteroidClusterRadiusPower);

            var maxIterations = 6;

            for (var i = 0; i < maxIterations; i++)
            {
                var position = TryGenerateAsteroidClusterPosition(
                    sector,
                    existingAsteroidClusters,
                    radius,
                    settings);

                if (position.HasValue)
                {
                    var asteroidCluster = PrefabHelper.Instantiate(asteroidClusterPrefab);
                    asteroidCluster.Radius = radius;
                    asteroidCluster.transform.SetParent(sector.transform, true);
                    asteroidCluster.transform.position = position.Value;
                    return asteroidCluster;
                }
            }

            return null;
        }

        private static Vector3? TryGenerateAsteroidClusterPosition(
            EditorSector sector,
            IEnumerable<EditorUnit> existingAsteroidClusters,
            float clusterRadius, 
            CustomSettings settings)
        {
            var maxDistanceFromOrigin = settings.MaxUnitDistanceFromOriginLowerBound;

            // Give a gutter
            maxDistanceFromOrigin -= 200.0f;

            Vector3 sectorPosition = Vector3.zero;

            maxDistanceFromOrigin -= clusterRadius;

            if (maxDistanceFromOrigin > 0.0f)
                sectorPosition = Geometry.RandomXZUnitVector() * (Random.Range(0.0f, maxDistanceFromOrigin));

            var newPosition = sector.transform.position + sectorPosition;

            if (existingAsteroidClusters != null && existingAsteroidClusters.Any())
            {
                foreach (var existingAsteroidCluster in existingAsteroidClusters)
                {
                    var minDistance = settings.MinDistanceBetweenAsteroidClusters + clusterRadius + existingAsteroidCluster.Radius;

                    if (Vector3.Distance(existingAsteroidCluster.transform.position, newPosition) < minDistance)
                        return null;
                }
            }

            return newPosition;
        }

        private struct Point
        {
            public int X;
            public int Y;
        }
    }
}
