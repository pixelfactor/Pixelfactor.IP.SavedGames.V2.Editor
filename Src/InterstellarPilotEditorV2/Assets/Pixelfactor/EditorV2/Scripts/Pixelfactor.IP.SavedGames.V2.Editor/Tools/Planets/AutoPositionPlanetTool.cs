using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Planets
{
    public static class AutoPositionPlanetTool
    {
        public static void AutoPositionPlanet(EditorUnit planetUnit, EditorSector sector, CustomSettings customSettings)
        {
            var localPosition = GetRandomPlanetLocalPosition(customSettings);

            planetUnit.transform.position = sector.transform.position + localPosition;

            var planetRotationX = Random.Range(customSettings.MinPlanetRotationX, customSettings.MaxPlanetRotationX) * RandomSign();
            planetUnit.transform.rotation = Quaternion.Euler(new Vector3(planetRotationX, Random.Range(0.0f, 360.0f), 0.0f));
        }

        public static float RandomSign()
        {
            if (Random.Range(0, 2) == 0)
            {
                return 1.0f;
            }
            return -1.0f;
        }

        public static Vector3 GetRandomPlanetLocalPosition(CustomSettings settings)
        {
            var yPosition = Random.Range(settings.MinPlanetPositionY, settings.MaxPlanetPositionY);

            var v = Geometry.RandomXZUnitVector();

            var range = Random.Range(settings.MinPlanetDistance, settings.MaxPlanetDistance);

            var position = v * range;

            position.y = yPosition;

            return position;
        }
    }
}
