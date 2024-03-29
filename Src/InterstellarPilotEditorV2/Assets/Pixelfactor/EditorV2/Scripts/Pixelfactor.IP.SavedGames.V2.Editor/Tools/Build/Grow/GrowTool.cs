﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Connect;
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
        public const int MaxIterations = 32;

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
            if (sectorPrefab == null)
            {
                throw new System.Exception("Sector prefab is required");
            }

            if (existingSector == null)
            {
                throw new System.Exception("Existing sector is required");
            }

            var allSectors = existingSector.GetSavedGame().GetSectors().ToList();
            var sectorLines = GrowHelper.GetSectorConnectionLines(allSectors);

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

                Debug.Log($"Spawned new sector: {sector.Name}", sector);
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
                ConnectSectorsTool.ConnectSectors(existingSector, newSector, existingSector.GetSavedGame().MaxWormholeDistance);
            }

            return newSector;
        }
    }
}
