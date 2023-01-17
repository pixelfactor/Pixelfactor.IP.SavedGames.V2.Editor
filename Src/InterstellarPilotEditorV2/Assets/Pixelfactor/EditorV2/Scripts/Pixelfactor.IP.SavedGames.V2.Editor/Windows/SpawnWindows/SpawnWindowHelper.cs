using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows
{
    public static class SpawnWindowHelper
    {
        public static Vector3 GetNewUnitSpawnPosition(EditorSector sector, float radius)
        {
            var initialPosition = GetCurrentScenePositionInScene();

            var newPosition = SpawnPositionFinder.FindPositionOrNull(sector, initialPosition, radius);
            if (newPosition.HasValue)
            {
                return newPosition.Value;
            }

            return sector.transform.position;
        }

        public static Vector3 GetSceneViewCenter()
        {
            return GetCurrentScenePositionInScene(SceneView.lastActiveSceneView.position.center);
        }

        public static Vector3 GetCurrentScenePositionInScene()
        {
            return GetCurrentScenePositionInScene(Event.current.mousePosition);
        }

        public static Vector3 GetCurrentScenePositionInScene(Vector2 screenSpace)
        {
            Vector3 mousePosition = screenSpace;
            var placeObject = HandleUtility.PlaceObject(mousePosition, out var newPosition, out var normal);
            var p = placeObject ? newPosition : HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10);
            p.y = 0.0f;
            return p;
        }
    }
}
