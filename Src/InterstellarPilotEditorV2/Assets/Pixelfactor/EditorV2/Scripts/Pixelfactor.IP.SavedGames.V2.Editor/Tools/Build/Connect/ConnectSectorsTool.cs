using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Connect
{
    public class ConnectSectorsTool
    {
        public static void ConnectSectorsToOthers(
            IEnumerable<EditorSector> sectorsToConnect,
            IEnumerable<EditorSector> allSectors,
            int maxWormholes,
            float connectionProbability,
            float minDistanceBetweenSectors,
            float maxDistanceBetweenSectors,
            float minAngleBetweenWormholes,
            float wormholeDistance)
        {
            var sectorLines = GrowHelper.GetSectorConnectionLines(allSectors.ToList());

            foreach (var sector in sectorsToConnect)
            {
                ConnectSectorToOthersInternal(
                    sector,
                    allSectors,
                    maxWormholes,
                    connectionProbability,
                    minDistanceBetweenSectors,
                    maxDistanceBetweenSectors,
                    minAngleBetweenWormholes,
                    wormholeDistance,
                    sectorLines);
            }
        }

        public static void ConnectSectorToOthers(
            EditorSector sector,
            IEnumerable<EditorSector> allSectors,
            int maxWormholes,
            float connectionProbability,
            float minDistanceBetweenSectors,
            float maxDistanceBetweenSectors,
            float minAngleBetweenWormholes,
            float wormholeDistance)
        {
            var sectorLines = GrowHelper.GetSectorConnectionLines(allSectors.ToList());

            ConnectSectorToOthersInternal(
                sector,
                allSectors,
                maxWormholes,
                connectionProbability,
                minDistanceBetweenSectors,
                maxDistanceBetweenSectors,
                minAngleBetweenWormholes,
                wormholeDistance,
                sectorLines);
        }

        private static void ConnectSectorToOthersInternal(
            EditorSector sector,
            IEnumerable<EditorSector> allSectors,
            int maxWormholes,
            float connectionProbability,
            float minDistanceBetweenSectors,
            float maxDistanceBetweenSectors,
            float minAngleBetweenWormholes,
            float wormholeDistance,
            List<Line> sectorLines)
        {
            var currentWormholes = sector.GetValidStableWormholes();
            var currentTargetSectors = currentWormholes.Select(e => e.GetActualTargetSector()).Where(e => e != null).ToList();

            if (currentWormholes.Count >= maxWormholes)
                return;

            foreach (var otherSector in allSectors)
            {
                if (otherSector != sector)
                {
                    var distance = Vector3.Distance(otherSector.transform.position, sector.transform.position);

                    if (distance > maxDistanceBetweenSectors)
                    {
                        continue;
                    }

                    if (currentTargetSectors.Contains(otherSector))
                    {
                        continue;
                    }

                    if (otherSector.ConnectionExistsAtPosition(sector.transform.position, minAngleBetweenWormholes))
                    {
                        continue;
                    }

                    if (sector.ConnectionExistsAtPosition(otherSector.transform.position, minAngleBetweenWormholes))
                    {
                        continue;
                    }

                    if (GrowHelper.DoesNewConnectionIntersect(
                        sectorLines,
                        sector.transform.position, 
                        otherSector.transform.position,
                        0.0f,
                        0.0f))
                    {
                        continue;
                    }

                    // Further the distance, the less likely to autoconnect
                    var relativeDistance = Mathf.Clamp01((distance - minDistanceBetweenSectors) / (maxDistanceBetweenSectors - minDistanceBetweenSectors));
                    var actualLikelihood = Mathf.Lerp(connectionProbability, connectionProbability * 0.2f, relativeDistance);
                    if (Random.value < actualLikelihood)
                    {
                        ConnectSectors(sector, otherSector, wormholeDistance);
                    }
                }
            }
        }

        /// <summary>
        /// TODO: This tool will always create two new wormholes, it won't look for existing ones first.
        /// </summary>
        public static void ConnectSelectedSectorsWithWormholesMenuItem()
        {
            var editorScenario = SavedGameUtil.FindSavedGameOrErrorOut();

            ConnectSelectedSectorsWithWormholes(editorScenario.MaxWormholeDistance);
        }

        public static bool CanConnectSelectedSectorsWithWormholes()
        {
            return TryGetSelectedSectors(out _);
        }

        private static void ConnectSelectedSectorsWithWormholes(float maxWormholeDistance)
        {
            if (!TryGetSelectedSectors(out List<EditorSector> sectors))
            {
                Logging.LogAndThrow("Expected to have 2 selected sectors");
            }
            ConnectSectors(sectors, maxWormholeDistance);
        }

        public static void ConnectSectors(EditorSector sector1, EditorSector sector2, float wormholeDistance)
        {
            ConnectSectors(new EditorSector[] { sector1, sector2 }.ToList(), wormholeDistance);
        }

        private static void ConnectSectors(List<EditorSector> selectedSectors, float wormholeDistance)
        {
            var wormhole1 = ConnectSectorTo(selectedSectors[0], selectedSectors[1], wormholeDistance * selectedSectors[0].WormholeDistance);
            var wormhole2 = ConnectSectorTo(selectedSectors[1], selectedSectors[0], wormholeDistance * selectedSectors[1].WormholeDistance);

            wormhole1.TargetWormholeUnit = wormhole2.GetComponent<EditorUnit>();
            wormhole2.TargetWormholeUnit = wormhole1.GetComponent<EditorUnit>();

            AutoNameObjects.AutoNameWormhole(wormhole1);
            AutoNameObjects.AutoNameWormhole(wormhole2);
        }

        private static EditorUnitWormholeData ConnectSectorTo(
            EditorSector editorSector1,
            EditorSector editorSector2,
            float wormholeDistance)
        {
            var newWormhole = Spawn.Unit(editorSector1, Model.ModelUnitClass.Wormhole_Default, CustomSettings.GetOrCreateSettings().UnitPrefabsPath);

            var direction = (editorSector2.transform.position - editorSector1.transform.position).normalized;
            newWormhole.transform.position = editorSector1.transform.position + (direction * wormholeDistance);
            newWormhole.transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);

            var newWormholeData = newWormhole.GetComponent<EditorUnitWormholeData>();
            return newWormholeData;
        }

        public static bool TryGetSelectedSectors(out List<EditorSector> sectors)
        {
            sectors = new List<EditorSector>();

            if (Selection.objects.Length != 2)
            {
                return false;
            }

            foreach (var obj in Selection.objects)
            {
                if (obj is GameObject gameObject)
                {
                    var editorSector = gameObject.GetComponent<EditorSector>();
                    if (editorSector != null)
                    {
                        sectors.Add(editorSector);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
