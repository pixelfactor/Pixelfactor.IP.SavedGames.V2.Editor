using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Sectors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class PlanetSpawnTool
    {
        public static List<EditorSector> GetNewPlanetSectors(List<EditorSector> sectors, CustomSettings settings)
        {
            var newPlanetSectors = new List<EditorSector>();
            var planetSectors = new List<EditorSector>();

            foreach (var sector in sectors)
            {
                if (sector.HasPlanets())
                {
                    planetSectors.Add(sector);  
                }
            }

            var totalWeighting = settings.PlanetSectorWeighting + settings.DeepSpaceSectorWeighting + settings.AsteroidSectorWeighting;

            var numPlanetSectors = Mathf.Max(
                settings.MinNumberPlanetSectors,
                Mathf.RoundToInt(settings.PlanetSectorWeighting / totalWeighting * sectors.Count()));

            var numExistingPlanetSectors = planetSectors.Count;

            for (int i = 0; i < numPlanetSectors - numExistingPlanetSectors; i++)
            {
                var bestSectorForPlanet = GetBestSectorForPlanet(sectors, planetSectors, settings);
                if (bestSectorForPlanet != null)
                {
                    planetSectors.Add(bestSectorForPlanet);
                    newPlanetSectors.Add(bestSectorForPlanet);
                }
            }

            return newPlanetSectors;
        }

        public static EditorSector GetBestSectorForPlanet(
            IEnumerable<EditorSector> sectors,
            IEnumerable<EditorSector> planetSectors,
            CustomSettings customSettings)
        {
            return sectors
                .Where(e => !planetSectors.Contains(e))
                .OrderByDescending(e => ScoreSectorForPlanet(sectors, planetSectors, e))
                .FirstOrDefault();
        }

        private static float ScoreSectorForPlanet(
            IEnumerable<EditorSector> sectors,
            IEnumerable<EditorSector> planetSectors,
            EditorSector e)
        {
            var score = 0.0f;

            var minDist = GetSectorMinDistanceFromPlanetSector(sectors, planetSectors, e);
            if (minDist.HasValue && minDist < 4)
            {
                score -= (4 - minDist.Value);
            }

            // Really don't want planet sectors this close
            if (minDist < 2)
            {
                score -= 10.0f;
            }

            // The more sector connections the better we like this as a planet sector
            score += e.GetStableWormholeCount();

            return score;
        }

        public static int? GetSectorMinDistanceFromPlanetSector(
            IEnumerable<EditorSector> sectors,
            IEnumerable<EditorSector> planetSectors,
            EditorSector sector)
        {
            var result = SectorFinder.Find(sector, sectors, (e) => planetSectors.Contains(e), 4);
            if (result != null)
            {
                return result.Value.jumpDistance;
            }

            return null;
        }
    }
}
