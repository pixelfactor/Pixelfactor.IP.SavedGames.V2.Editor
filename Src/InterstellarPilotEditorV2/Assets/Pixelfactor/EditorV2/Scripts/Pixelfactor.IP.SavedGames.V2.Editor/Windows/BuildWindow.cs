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
        private GrowSectorSelectionMode growSectorSelectionMode = GrowSectorSelectionMode.Keep;
        private int growMaxWormholeConnections = 8;
        private float sectorDistanceFuzziness = 0.5f;

        public void Draw()
        {
            DrawConnectOptions();
            GuiHelper.SectionSpace();

            DrawGrowOptions();
            GuiHelper.SectionSpace();

            DrawResizeOptions();
            GuiHelper.SectionSpace();
        }

        private static void DrawConnectOptions()
        {
            GuiHelper.Subtitle("Connect", "Create wormholes between selected sectors");

            var enabled = ConnectSectorsTool.CanConnectSelectedSectorsWithWormholes();

            EditorGUI.BeginDisabledGroup(!enabled);

            if (GUILayout.Button(
                new GUIContent(
                    "Connect selected sectors",
                    "Connects the currently selected sectors with wormholes"),
                Styles.Button))
            {
                ConnectSectorsTool.ConnectSelectedSectorsWithWormholesMenuItem();
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DrawGrowOptions()
        {
            var settings = CustomSettings.GetOrCreateSettings();

            var sectors = Selector.GetInParents<EditorSector>().ToList();

            var hasSectors = sectors.Any();

            GuiHelper.Subtitle("Grow", "Create new sectors");

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Selected sectors", WindowHelper.DescribeSectors(sectors));
            EditorGUI.EndDisabledGroup();

            if (newSectorPrefab == null)
            {
                newSectorPrefab = AssetDatabase.LoadAssetAtPath<EditorSector>(settings.SectorPrefabPath);
            }

            newSectorPrefab = (EditorSector)EditorGUILayout.ObjectField("Sector prefab", newSectorPrefab, typeof(EditorSector), allowSceneObjects: false);

            EditorGUILayout.PrefixLabel(new GUIContent("Selection mode", "Determines what to select after creating new sectors"));

            this.growSectorSelectionMode = (GrowSectorSelectionMode)EditorGUILayout.EnumPopup(
                this.growSectorSelectionMode,
                GUILayout.ExpandWidth(false));

            EditorGUILayout.PrefixLabel(new GUIContent("Max wormholes", "The max number of stable wormholes that a sector should have"));
            this.growMaxWormholeConnections = EditorGUILayout.IntSlider(this.growMaxWormholeConnections, 2, 8, GUILayout.ExpandWidth(false));

            EditorGUILayout.PrefixLabel(new GUIContent("Sector distance fuzziness", "How much to tweak the distance between sectors"));
            this.sectorDistanceFuzziness = EditorGUILayout.Slider(this.sectorDistanceFuzziness, 0.0f, 1.0f, GUILayout.ExpandWidth(false));

            EditorGUI.BeginDisabledGroup(!hasSectors && newSectorPrefab != null);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(
                new GUIContent(
                    "Grow each once",
                    "Adds a single sector to each of the selected sectors"),
                Styles.Button))
            {
                GrowEachOnce(sectors, settings);
            }

            if (GUILayout.Button(
                new GUIContent(
                    "Grow once randomly",
                    "Adds a sector to one of the selected sectors"),
                Styles.Button))
            {
                GrowOneRandomly(sectors, settings);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
        }

        private float GetNewSectorDistance(CustomSettings customSettings)
        {
            return Mathf.Lerp(
                customSettings.MinDistanceBetweenSectors,
                customSettings.MaxDistanceBetweenSectors,
                Mathf.Pow(Random.value, Mathf.Lerp(8.0f, 1.0f, this.sectorDistanceFuzziness)));
        }

        private void GrowOneRandomly(List<EditorSector> selectedSectors, CustomSettings settings)
        {
            var randomSector = selectedSectors.Where(e => CanGrowSector(e)).GetRandom();
            if (randomSector != null)
            {
                var newSector = GrowTool.GrowOnceAndConnect(
                    randomSector,
                    newSectorPrefab,
                    GetNewSectorDistance(settings),
                    settings.MinDistanceBetweenSectors,
                    settings.MinAngleBetweenWormholes);

                if (newSector != null)
                {
                    ApplyGrowSelectionMode(selectedSectors, newSector);
                }
                else
                {
                    EditorUtility.DisplayDialog("Grow sectors", "None of the selected sectors could be grown", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Grow sectors", "None of the selected sectors could be grown", "OK");
            }
        }

        private void GrowEachOnce(List<EditorSector> selectedSectors, CustomSettings settings)
        {
            var newSectors = new List<EditorSector>();

            foreach (var sector in selectedSectors)
            {
                if (CanGrowSector(sector))
                {
                    var newSector = GrowTool.GrowOnceAndConnect(
                        sector,
                        newSectorPrefab,
                        GetNewSectorDistance(settings),
                        settings.MinDistanceBetweenSectors,
                        settings.MinAngleBetweenWormholes);

                    if (newSector != null)
                    {
                        newSectors.Add(newSector);
                    }
                }
            }

            ApplyGrowSelectionMode(selectedSectors, newSectors.ToArray());
        }

        private void ApplyGrowSelectionMode(List<EditorSector> selectedSectors, params EditorSector[] newSectors)
        {
            switch (this.growSectorSelectionMode)
            {
                case GrowSectorSelectionMode.New:
                    Selection.objects = newSectors.Select(e => e.gameObject).ToArray();
                    break;
                case GrowSectorSelectionMode.Combined:
                    foreach (var newSector in newSectors)
                    {
                        selectedSectors.Add(newSector);
                    }
                    Selection.objects = selectedSectors.Select(e => e.gameObject).ToArray();
                    break;
            }
        }

        private bool CanGrowSector(EditorSector sector)
        {
            return sector.GetStableWormholeCount() < this.growMaxWormholeConnections;
        }

        private void DrawResizeOptions()
        {
            GuiHelper.Subtitle("Resize", "Increase/decrease distance between sectors");

            var sectors = SavedGameUtil.FindSectors();

            var enabled = sectors.Count > 1;

            EditorGUILayout.PrefixLabel(new GUIContent("Size multiplier", "The multiplier applied to the distance of existing sectors from the universe center"));
            this.expandMultiplier = EditorGUILayout.Slider(this.expandMultiplier, 0.01f, 2.0f, GUILayout.ExpandWidth(false));

            EditorGUI.BeginDisabledGroup(!enabled);

            if (GUILayout.Button(
                new GUIContent(
                    "Expand",
                    "Grows or shrinks the distance between existing sectors"),
                Styles.Button))
            {
                ExpandTool.Expand(this.expandMultiplier);
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
