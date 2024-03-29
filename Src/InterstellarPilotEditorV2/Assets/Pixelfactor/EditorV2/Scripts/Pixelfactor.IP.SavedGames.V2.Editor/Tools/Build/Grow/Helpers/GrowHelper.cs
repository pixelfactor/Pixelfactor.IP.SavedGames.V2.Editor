﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow
{
    public static class GrowHelper
    {
        public static bool IsPositionTooCloseToSectors(IEnumerable<EditorSector> sectors, Vector3 newPosition, float minDistanceBetweenSectors)
        {
            foreach (Vector3 sectorPosition in sectors.Select(e => e.transform.position))
            {
                if (Vector3.Distance(sectorPosition, newPosition) < minDistanceBetweenSectors)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether a proposed sector position would intersect an existing connection between two sectors
        /// </summary>
        /// <param name="sectorLines"></param>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="lengthAddition"></param>
        /// <param name="otherLineLengthAddition"></param>
        /// <returns></returns>
        public static bool DoesNewConnectionIntersect(
            IEnumerable<Line> sectorLines,
            Vector3 pos1,
            Vector3 pos2,
            float lengthAddition,
            float otherLineLengthAddition)
        {
            var dir = Vector3.Normalize(pos2 - pos1);

            // Give a bit of tolerance to the start and end position to avoid other lines that start in the same position but don't necesarily intersect
            pos1 += dir * 1.0f;
            pos2 -= dir * 1.0f;

            var p1 = new Vector2(pos1.x, pos1.z);
            var p2 = new Vector2(pos2.x, pos2.z);

            if (lengthAddition > 0.0f)
            {
                ExtendLine(p1, p2, lengthAddition, out p1, out p2);
            }

            foreach (var e in sectorLines)
            {
                var ep1 = e.P1;
                var ep2 = e.P2;

                if (otherLineLengthAddition > 0.0f)
                {
                    ExtendLine(ep1, ep2, otherLineLengthAddition, out ep1, out ep2);
                }

                if (Line.Intersection(ep1, ep2, p1, p2).HasValue)
                    return true;
            }

            return false;
        }

        private static void ExtendLine(
            Vector2 p1,
            Vector2 p2,
            float lengthAddition,
            out Vector2 result1,
            out Vector2 result2)
        {
            var v = p2 - p1;
            var vn = v.normalized;

            result1 = p1 - (vn * lengthAddition);
            result2 = p2 + (vn * lengthAddition);
        }

        public static List<Line> GetSectorConnectionLines(List<EditorSector> allSectors)
        {
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

            return sectorLines;
        }
    }
}
