using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit;
using System.Collections.Generic;
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
            float distanceBetweenSectors,
            float minAngleBetweenWormholes)
        {
            for (int i = 0; i < MaxIterations; i++)
            {
                var newPosition = existingSector.transform.position + Geometry.RandomXZUnitVector() * distanceBetweenSectors;

                if (existingSector.ConnectionExistsAtPosition(newPosition, minAngleBetweenWormholes))
                    continue;

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
            float distanceBetweenSectors,
            float minAngleBetweenWormhole)
        {
            var newSector = GrowOnce(existingSector, sectorPrefab, distanceBetweenSectors, minAngleBetweenWormhole);
            if (newSector != null)
            {
                ConnectSectorsTool.ConnectSectors(existingSector, newSector, existingSector.GetSavedGame().PreferredWormholeDistance);
            }

            return newSector;
        }
    }
}
