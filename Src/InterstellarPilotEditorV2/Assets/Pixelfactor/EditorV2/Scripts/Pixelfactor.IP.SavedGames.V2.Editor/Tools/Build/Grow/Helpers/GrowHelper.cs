using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow
{
    public static class GrowHelper
    {
        public static bool PositionTooCloseToSectors(IEnumerable<EditorSector> sectors, Vector3 newPosition, float minDistanceBetweenSectors)
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
    }
}
