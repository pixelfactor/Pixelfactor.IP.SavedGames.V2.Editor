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
    }
}
