﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow
{
    /// <summary>
    /// Adds sectors to an existing one
    /// </summary>
    public static class GrowTool
    {
        /// <summary>
        /// Max iterations before giving up trying to create a new wormhole
        /// </summary>
        public const int MaxIterations = 10;

        /// <summary>
        /// Adds a new connection to the existing sector. Returns null if there's no space
        /// </summary>
        /// <param name="editorSector"></param>
        /// <returns></returns>
        public static EditorSector GrowOnce(
            EditorSector existingSector,
            EditorSector sectorPrefab,
            float preferredDistance,
            float minDistanceBetweenSectors,
            float minAngleBetweenWormholes)
        {
            var allSectors = existingSector.GetSavedGame().GetSectors().ToList();

            List<Line> sectorLines = new List<Line>(allSectors.Count * 2);
            foreach (var sector in allSectors)
            {
                var wormholes = sector.GetValidStableWormholes();
                foreach (var wormhole in wormholes)
                {
                    var targetPosition = wormhole.GetActualTargetSector().transform.position;

                    var line = new Line(
                        new Vector2(sector.transform.position.x, sector.transform.position.z),
                        new Vector2(targetPosition.x, targetPosition.z));

                    sectorLines.Add(line);
                }
            }

            for (int i = 0; i < MaxIterations; i++)
            {
                var newPosition = existingSector.transform.position + Geometry.RandomXZUnitVector() * preferredDistance;

                if (existingSector.ConnectionExistsAtPosition(newPosition, minAngleBetweenWormholes))
                { 
                    continue;
                }

                if (GrowHelper.IsPositionTooCloseToSectors(allSectors, newPosition, minDistanceBetweenSectors))
                {
                    continue;
                }

                if (GrowHelper.DoesNewConnectionIntersect(sectorLines, existingSector.transform.position, newPosition, 1000.0f, 0.0f))
                {
                    continue;
                }

                var newInstance = (GameObject)PrefabUtility.InstantiatePrefab(sectorPrefab.gameObject, existingSector.transform.parent);
                var sector = newInstance.GetComponent<EditorSector>();

                sector.transform.position = newPosition;

                EditSectorTool.Randomize(sector);

                return sector;
            }

            return null;
        }

        public static EditorSector GrowOnceAndConnect(
            EditorSector existingSector,
            EditorSector sectorPrefab,
            float preferredDistance,
            float minDistanceBetweenSectors,
            float minAngleBetweenWormhole)
        {
            var newSector = GrowOnce(existingSector, sectorPrefab, preferredDistance, minDistanceBetweenSectors, minAngleBetweenWormhole);
            if (newSector != null)
            {
                ConnectSectorsTool.ConnectSectors(existingSector, newSector, existingSector.GetSavedGame().PreferredWormholeDistance);
            }

            return newSector;
        }
    }
}
