using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Expand;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class BuildWindow
    {
        private EditorSector newSectorPrefab = null;
        private float expandMultiplier = 2.0f;
        private GrowSectorSelectionMode growSectorSelectionMode = GrowSectorSelectionMode.Retain;

        public void Draw()
        {
            DrawConnectOptions();
            EditorGUILayout.Space();

            DrawGrowOptions();
            EditorGUILayout.Space();

            DrawExpandOptions();
            EditorGUILayout.Space();
        }

        private static void DrawConnectOptions()
        {
            EditorGUILayout.LabelField("Connect", EditorStyles.boldLabel);

            var enabled = ConnectSectorsTool.CanConnectSelectedSectorsWithWormholes();

            EditorGUI.BeginDisabledGroup(!enabled);

            if (GUILayout.Button(new GUIContent(
                "Connect selected sectors",
                "Connects the currently selected sectors with wormholes")))
            {
                ConnectSectorsTool.ConnectSelectedSectorsWithWormholesMenuItem();
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DrawGrowOptions()
        {
            var sectors = Selector.GetInParents<EditorSector>().ToList();

            var hasSectors = sectors.Any();

            EditorGUILayout.LabelField("Grow", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Selected sectors", WindowHelper.DescribeSectors(sectors));
            EditorGUI.EndDisabledGroup();

            if (newSectorPrefab == null)
            {
                var settings = CustomSettings.GetOrCreateSettings();
                newSectorPrefab = AssetDatabase.LoadAssetAtPath<EditorSector>(settings.SectorPrefabPath);
            }

            newSectorPrefab = (EditorSector)EditorGUILayout.ObjectField("Sector prefab", newSectorPrefab, typeof(EditorSector), allowSceneObjects: false);

            this.growSectorSelectionMode = (GrowSectorSelectionMode)GUILayout.SelectionGrid(
                (int)this.growSectorSelectionMode,
                System.Enum.GetNames(typeof(GrowSectorSelectionMode)),
                System.Enum.GetNames(typeof(GrowSectorSelectionMode)).Length);

            EditorGUI.BeginDisabledGroup(!hasSectors && newSectorPrefab != null);

            if (GUILayout.Button(new GUIContent(
                "Grow once",
                "Adds a single sector to the existing selection")))
            {
                var newSectors = new List<EditorSector>();

                foreach (var sector in sectors)
                {
                    var newSector = GrowTool.GrowOnceAndConnect(sector, newSectorPrefab);

                    if (newSector != null)
                    {
                        newSectors.Add(newSector);
                    }
                }

                switch (this.growSectorSelectionMode)
                {
                    case GrowSectorSelectionMode.New:
                        Selection.objects = newSectors.Select(e => e.gameObject).ToArray();
                        break;
                    case GrowSectorSelectionMode.Combined:
                        foreach (var newSector in newSectors)
                        {
                            sectors.Add(newSector);
                        }
                        Selection.objects = sectors.Select(e => e.gameObject).ToArray();
                        break;
                }
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DrawExpandOptions()
        {
            EditorGUILayout.LabelField("Expand", EditorStyles.boldLabel);

            var sectors = SavedGameUtil.FindSectors();

            var enabled = sectors.Count > 1;

            var content = new GUIContent("Expansion", "The multiplier applied to the distance of existing sectors from the universe center");
            this.expandMultiplier = EditorGUILayout.Slider(content, this.expandMultiplier, 0.01f, 2.0f);

            EditorGUI.BeginDisabledGroup(!enabled);

            if (GUILayout.Button(new GUIContent(
                "Expand",
                "Grows or shrinks the distance between existing sectors")))
            {
                ExpandTool.Expand(this.expandMultiplier);
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
