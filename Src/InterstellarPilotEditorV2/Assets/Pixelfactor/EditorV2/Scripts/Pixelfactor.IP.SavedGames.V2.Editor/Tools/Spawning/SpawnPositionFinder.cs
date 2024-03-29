﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    /// <summary>
    /// Basic algorithm that finds a safe place to spawn something without intersecting
    /// </summary>
    public static class SpawnPositionFinder
    {
        public static int NumDirectionChecks = 6;

        const float IncrementMultiplier = 1.5f;

        public static bool IsPositionWithinBounds(EditorSector editorSector, Vector3 position, float maxDistance)
        {
            var localPosition = position - editorSector.transform.position;
            return IsLocalPositionWithinBounds(maxDistance, localPosition);
        }

        public static bool IsLocalPositionWithinBounds(float maxDistance, Vector3 localPosition, bool checkYComponent = false)
        {
            if (checkYComponent)
            {
                if (Mathf.Abs(localPosition.y) > 0.1f)
                    return false;
            }

            if (localPosition.x < -maxDistance || localPosition.x > maxDistance)
                return false;

            if (localPosition.z < -maxDistance || localPosition.z > maxDistance)
                return false;

            return true;
        }

        public static Vector3? FindPositionOrNull(
            EditorSector sector,
            Vector3 checkPosition,
            float radius,
            int iterations = 4)
        {
            var layerMask = ~0;
            var settings = CustomSettings.GetOrCreateSettings();
            var maxSectorDistance = settings.MaxUnitDistanceFromOriginUpperBound - radius;

            // If not already overlapping then were done
            if (IsOverlapping(checkPosition, radius, layerMask))
            {
                // Slightly increase the radius to allow some tolerance
                radius *= 1.1f;

                var currentOffsetFromCheckPosition = Mathf.Max(10.0f, radius * IncrementMultiplier);

                // Randomize the first direction picked so that we don't place return similar results
                var startDirectionIndex = Random.Range(0, NumDirectionChecks);

                for (int i = 0; i < iterations; i++)
                {
                    for (int j = 0; j < NumDirectionChecks; j++)
                    {
                        var directionIndex = MiscUtils.WrapValue(startDirectionIndex + j, 0, NumDirectionChecks);

                        var newPos = checkPosition +
                            Quaternion.Euler(0.0f, directionIndex / (float)NumDirectionChecks * 360.0f, 0.0f) * (Vector3.forward * currentOffsetFromCheckPosition);

                        if (!IsPositionWithinBounds(sector, newPos, maxSectorDistance))
                            continue;

                        if (!IsOverlapping(newPos, radius, layerMask))
                        {
                            return newPos;
                        }
                    }

                    currentOffsetFromCheckPosition += ((40.0f + radius) * IncrementMultiplier);
                }

                Debug.LogWarning($"Max attempts {iterations} reached when trying to find safe deployment position in {sector.Name} at position {checkPosition} with radius: {radius}");

                return null;
            }

            return checkPosition;
        }

        /// <summary>
        /// Returns true if there are not units overlapping the position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool IsOverlapping(Vector3 position, float radius, LayerMask layerMask)
        {
            return Physics.CheckSphere(position, radius, layerMask, QueryTriggerInteraction.Collide);
        }
    }
}
