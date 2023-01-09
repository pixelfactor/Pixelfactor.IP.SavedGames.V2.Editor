using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Expand
{
    /// <summary>
    /// Grows or shrinks the distance between sectors
    /// </summary>
    public static class ExpandTool
    {
        public static void Expand(float distanceMultiplier = 2.0f)
        {
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();

            var sectors = editorSavedGame.GetComponentsInChildren<EditorSector>();

            var center = GetSectorsCenter(sectors);

            foreach (var sector in sectors)
            {
                var v = sector.transform.position - center;

                v *= distanceMultiplier;

                sector.transform.position = v;
                EditorUtility.SetDirty(sector);
            }
        }

        public static Vector3 GetSectorsCenter(IEnumerable<EditorSector> sectors)
        {
            var minX = sectors.Min(e => e.transform.position.x);
            var maxX = sectors.Max(e => e.transform.position.x);
            var minZ = sectors.Min(e => e.transform.position.z);
            var maxZ = sectors.Max(e => e.transform.position.z);

            return new Vector3(
                minX + (maxX - minX) / 2.0f,
                0.0f,
                minZ + (maxZ - minZ) / 2.0f);
        }
    }
}
