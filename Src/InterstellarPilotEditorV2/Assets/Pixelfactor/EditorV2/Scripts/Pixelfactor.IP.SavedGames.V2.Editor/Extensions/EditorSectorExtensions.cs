using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class EditorSectorExtensions
    {
        public static EditorScenario GetSavedGame(this EditorSector editorSector)
        {
            return editorSector.GetComponentInParent<EditorScenario>();
        }

        public static List<EditorUnitWormholeData> GetValidStableWormholes(this EditorSector editorSector)
        {
            return editorSector.GetComponentsInChildren<EditorUnitWormholeData>().Where(e => !e.IsUnstable && e.TargetWormholeUnit != null).ToList();
        }

        public static int GetStableWormholeCount(this EditorSector editorSector)
        {
            return GetValidStableWormholes(editorSector).Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editorSector"></param>
        /// <param name="targetSector"></param>
        /// <param name="minAngleBetweenWormholes"></param>
        /// <returns></returns>
        public static bool ConnectionExistsAtPosition(this EditorSector editorSector, Vector3 targetPosition, float minAngleBetweenWormholes)
        {
            var direction = Vector3.Normalize(targetPosition - editorSector.transform.position);
            return ConnectionExistsAtDirection(editorSector, direction, minAngleBetweenWormholes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editorSector"></param>
        /// <param name="targetDirection"></param>
        /// <param name="minAngleBetweenWormholes"></param>
        /// <returns></returns>
        public static bool ConnectionExistsAtDirection(this EditorSector editorSector, Vector3 targetDirection, float minAngleBetweenWormholes)
        {
            var wormholes = editorSector.GetValidStableWormholes();

            for (int i = 0; i < wormholes.Count; i++)
            {
                if (wormholes[i].TargetWormholeUnit == null)
                    continue;

                var connectionDirection = Vector3.Normalize(
                    wormholes[i].TargetWormholeUnit.GetComponentInParent<EditorSector>().transform.position - editorSector.transform.position);

                var angle = Mathf.Abs(Vector3.Angle(targetDirection, connectionDirection));

                if (angle < (minAngleBetweenWormholes) / 2.0f)
                    return true;
            }

            return false;
        }
    }
}
