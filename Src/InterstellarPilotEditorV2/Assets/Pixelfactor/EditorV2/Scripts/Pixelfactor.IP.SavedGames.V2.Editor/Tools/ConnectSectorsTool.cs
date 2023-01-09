using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public class ConnectSectorsTool
    {
        /// <summary>
        /// TODO: Move this to settings
        /// </summary>
        const string wormholePrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/Units/Wormholes/Unit_Wormhole.prefab";

        /// <summary>
        /// TODO: This tool will always create two new wormholes, it won't look for existing ones first.
        /// </summary>
        public static void ConnectSelectedSectorsWithWormholesMenuItem()
        {
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();

            ConnectSelectedSectorsWithWormholes(editorSavedGame.PreferredWormholeDistance);
        }

        public static bool CanConnectSelectedSectorsWithWormholes()
        {
            return TryGetSelectedSectors(out _);
        }

        private static void ConnectSelectedSectorsWithWormholes(float wormholeDistance)
        {
            if (!TryGetSelectedSectors(out List<EditorSector> sectors))
            {
                Logging.LogAndThrow("Expected to have 2 selected sectors");
            }
            ConnectSectors(sectors, wormholeDistance);
        }

        private static void ConnectSectors(List<EditorSector> selectedSectors, float wormholeDistance)
        {
            var wormhole1 = ConnectSectorTo(selectedSectors[0], selectedSectors[1], wormholeDistance * selectedSectors[0].WormholeDistanceMultiplier);
            var wormhole2 = ConnectSectorTo(selectedSectors[1], selectedSectors[0], wormholeDistance * selectedSectors[1].WormholeDistanceMultiplier);

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
            var wormholePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(wormholePrefabPath);

            if (wormholePrefab == null)
            {
                throw new System.Exception($"Unable to load wormhole prefab. Check that the asset exists at path: \"{wormholePrefabPath}\"");
            }

            var newWormhole = (GameObject)PrefabUtility.InstantiatePrefab(wormholePrefab.gameObject);
            newWormhole.transform.SetParent(editorSector1.transform, false);

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
