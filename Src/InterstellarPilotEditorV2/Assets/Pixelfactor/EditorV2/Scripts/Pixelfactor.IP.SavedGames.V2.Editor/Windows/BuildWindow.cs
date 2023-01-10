using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Connect;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Expand;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
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
        private GrowSectorSelectionMode growSectorSelectionMode = GrowSectorSelectionMode.Combined;
        private int growMaxWormholeConnections = 8;
        private float sectorDistanceFuzziness = 0.5f;
        private bool autoConnect = true;
        private float autoConnectLikelihood = 0.5f;
        private int autoGrowSectorCount = 32;
        private bool hasConfirmedAutoGrowTrash = false;

        public void Draw()
        {
            DrawGrowOptions();
            GuiHelper.SectionSpace();

            DrawConnectOptions();
            GuiHelper.SectionSpace();

            DrawResizeOptions();
            GuiHelper.SectionSpace();
        }

        private void DrawConnectOptions()
        {
            GuiHelper.Subtitle("Connect", "Create wormholes between selected sectors");

            var enabled = ConnectSectorsTool.CanConnectSelectedSectorsWithWormholes();

            EditorGUI.BeginDisabledGroup(!enabled);

            if (GUILayout.Button(
                new GUIContent(
                    "Connect selected sectors",
                    "Connects the currently selected sectors with wormholes"),
                GuiHelper.ButtonLayout))
            {
                ConnectSectorsTool.ConnectSelectedSectorsWithWormholesMenuItem();
            }

            EditorGUI.EndDisabledGroup();

            var selectedSectors = Selector.GetInParents<EditorSector>().ToList();
            var autoConnectEnabled = selectedSectors.Count > 0;

            EditorGUILayout.PrefixLabel(new GUIContent("Auto-connect likelihood", "How likely new sectors will be connected to others"));
            this.autoConnectLikelihood = EditorGUILayout.Slider(this.autoConnectLikelihood, 0.0f, 1.0f, GUILayout.ExpandWidth(false));

            EditorGUI.BeginDisabledGroup(!autoConnectEnabled);

            if (GUILayout.Button(
                new GUIContent(
                    "Auto-connect selected sectors",
                    "Connects any selected sectors if needed"),
                GuiHelper.ButtonLayout))
            {
                var savedGame = SavedGameUtil.FindSavedGameOrErrorOut();
                var settings = CustomSettings.GetOrCreateSettings();

                foreach (var sector in selectedSectors)
                {
                    this.ApplyAutoConnectToSector(sector, settings, savedGame);
                }
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DrawGrowOptions()
        {
            var settings = CustomSettings.GetOrCreateSettings();

            var allSectors = SavedGameUtil.FindSavedGame().GetSectors().ToList();
            var selectedSectors = Selector.GetInParents<EditorSector>().ToList();

            var hasSectors = selectedSectors.Any();

            GuiHelper.Subtitle("Grow", "Create new sectors");

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Total sectors", $"{allSectors.Count:N0}");
            EditorGUILayout.TextField("Selected sectors", WindowHelper.DescribeSectors(selectedSectors));
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

            EditorGUILayout.PrefixLabel(new GUIContent("Auto-connect", "Whether to automatically connect new sectors to others"));
            this.autoConnect = EditorGUILayout.Toggle(this.autoConnect, GUILayout.ExpandWidth(false));

            EditorGUI.BeginDisabledGroup(!hasSectors && newSectorPrefab != null);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(
                new GUIContent(
                    "Grow",
                    "Creates new sectors connected to each of the selected sectors"),
                GuiHelper.ButtonLayout))
            {
                GrowEachOnce(selectedSectors, settings);
            }

            if (GUILayout.Button(
                new GUIContent(
                    "Grow by 1",
                    "Creates a new sector and connects to one of the selected sectors"),
                GuiHelper.ButtonLayout))
            {
                var newSector = GrowOneRandomly(selectedSectors, settings);

                if (newSector != null)
                {
                    ApplyAutoConnect(newSector, settings);

                    ApplyGrowSelectionMode(selectedSectors, newSector);
                }

                if (newSector == null)
                {
                    EditorUtility.DisplayDialog("Grow sectors", "None of the selected sectors could be grown", "OK");
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();

            DrawAutoGrow(settings, allSectors, selectedSectors);
        }

        private void DrawAutoGrow(CustomSettings settings, List<EditorSector> allSectors, List<EditorSector> selectedSectors)
        {
            GuiHelper.Subtitle("Auto-grow", "Create many new sectors");

            EditorGUI.BeginDisabledGroup(!selectedSectors.Any());

            EditorGUILayout.PrefixLabel(new GUIContent("Auto-grow count", "The sector count to grow to"));
            this.autoGrowSectorCount = EditorGUILayout.IntSlider(this.autoGrowSectorCount, 8, 256, GUILayout.ExpandWidth(false));

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(
                new GUIContent(
                    "Auto-grow",
                    "Keeps adding sectors to the selected sectors until a sector count is reached"),
                GuiHelper.ButtonLayout))
            {
                AutoGrow(selectedSectors, allSectors, settings, this.autoGrowSectorCount);
            }

            EditorGUI.EndDisabledGroup();

            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;

            if (GUILayout.Button(

                new GUIContent(
                    "Delete and Auto-grow",
                    "Deletes everything and grows a new universe"),

                GuiHelper.ButtonLayout))
            {
                if (this.hasConfirmedAutoGrowTrash || EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to delete everything?", "Yes", "No"))
                {
                    this.hasConfirmedAutoGrowTrash = true;

                    Selection.objects = new Object[0];
                    var savedGame = TrashAndRecreateScene(settings);

                    allSectors = savedGame.GetSectors().ToList();
                    selectedSectors = Selector.GetInParents<EditorSector>().ToList();

                    AutoGrow(selectedSectors, allSectors, settings, this.autoGrowSectorCount - 1);
                }
            }

            GUI.backgroundColor = oldColor;

            EditorGUILayout.EndHorizontal();
        }

        private EditorSavedGame TrashAndRecreateScene(CustomSettings settings)
        {
            var savedGame = SavedGameUtil.FindSavedGame();
            GameObject.DestroyImmediate(savedGame.gameObject);

            var newSavedGame = CreateNewScenarioTool.InstantiateSavedGame(settings.EmptyScenePrefabPath);

            Spawn.NewSector(newSavedGame, settings.SectorPrefabPath);

            Selector.SelectFirstSector(newSavedGame);

            return newSavedGame;
        }

        private float GetNewSectorDistance(CustomSettings customSettings)
        {
            return Mathf.Lerp(
                customSettings.MinDistanceBetweenSectors,
                customSettings.MaxDistanceBetweenSectors,
                Mathf.Pow(Random.value, Mathf.Lerp(32.0f, 1.0f, this.sectorDistanceFuzziness)));
        }

        private void AutoGrow(List<EditorSector> selectedSectors, List<EditorSector> allSectors, CustomSettings settings, int autoGrowCount)
        {
            var sectorCount = allSectors.Count;
            var createdCount = 0;

            const int maxFailedIterations = 64;
            int i = 0;
            while (createdCount < autoGrowCount && i < maxFailedIterations)
            {
                var newSector = GrowOneRandomly(selectedSectors, settings);
                if (newSector != null)
                {
                    ApplyAutoConnect(newSector, settings);

                    selectedSectors.Add(newSector);

                    createdCount++;
                    sectorCount++;
                }
                else
                {
                    i++;
                }
            }
        }

        private EditorSector GrowOneRandomly(List<EditorSector> selectedSectors, CustomSettings settings)
        {
            EditorSector newSector = null;

            var availableSectors = selectedSectors.Where(e => CanGrowSector(e));
            if (availableSectors.Count() > 0)
            {
                // Keep trying until find a sector to generate from
                for (int i = 0; i < 20; i++)
                {
                    var randomSector = availableSectors.GetRandom();
                    if (randomSector != null)
                    {
                        newSector = GrowTool.GrowOnceAndConnect(
                            randomSector,
                            newSectorPrefab,
                            GetNewSectorDistance(settings),
                            settings.MinDistanceBetweenSectors,
                            settings.MinAngleBetweenWormholes);

                        if (newSector != null)
                        {
                            break;
                        }
                    }
                }
            }

            return newSector;
        }

        private void ApplyAutoConnect(
            EditorSector newSector,
            CustomSettings customSettings)
        {
            if (!this.autoConnect)
                return;

            var savedGame = newSector.GetSavedGame();

            ApplyAutoConnectToSector(newSector, customSettings, savedGame);
        }

        private void ApplyAutoConnectToSector(
            EditorSector newSector, 
            CustomSettings customSettings, 
            EditorSavedGame savedGame)
        {
            ConnectSectorsTool.ConnectSectorToOthers(
                newSector,
                savedGame.GetSectors(),
                this.growMaxWormholeConnections,
                Mathf.Lerp(0.1f, 1.0f, this.autoConnectLikelihood),
                customSettings.MinDistanceBetweenSectors * 0.4f, // Increase tolerance slightly
                customSettings.MaxDistanceBetweenSectors * 1.1f,
                customSettings.MinAngleBetweenWormholes,
                newSector.WormholeDistanceMultiplier * savedGame.MaxWormholeDistance);
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
                        ApplyAutoConnect(newSector, settings);

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
                    if (newSectors.Any())
                    { 
                        Selection.objects = newSectors.Select(e => e.gameObject).ToArray();
                    }
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
                GuiHelper.ButtonLayout))
            {
                ExpandTool.Expand(this.expandMultiplier);
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
