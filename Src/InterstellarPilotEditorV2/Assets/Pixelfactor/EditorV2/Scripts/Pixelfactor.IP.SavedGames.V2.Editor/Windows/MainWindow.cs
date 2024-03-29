﻿using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class MainWindow
    {
        public void Draw()
        {
            EditorGUILayout.Space();

            var editorScenario = SavedGameUtil.FindSavedGame();
            var hasScenario = editorScenario != null;
            var canPlay = hasScenario && !string.IsNullOrWhiteSpace(CustomSettings.GetOrCreateSettings().GameExecutablePath);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Current scenario", editorScenario != null ? editorScenario.Title : "[None]");
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space(30);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            DrawButtons(editorScenario, hasScenario, canPlay);
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawButtons(EditorScenario editorScenario, bool hasScenario, bool canPlay)
        {
            EditorGUI.BeginDisabledGroup(!canPlay);

            if (GUILayout.Button(
                new GUIContent(
                    "Play",
                    "Plays the scenario. If the button is disabled, configure the exe path in Settings"),
                GuiHelper.ButtonLayout))
            {
                PlayTool.Play();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!hasScenario);

            if (GUILayout.Button(
                new GUIContent(
                    "Quick export",
                    "Exports the scenario to the location defined in project settings"),
                GuiHelper.ButtonLayout))
            {
                ImportExportTool.FixUpValidateAndExport();
            }

            if (GUILayout.Button(
                new GUIContent(
                    "Export to...",
                    "Exports the scenario to a specific location"),
                GuiHelper.ButtonLayout))
            {
                ImportExportTool.FixUpAndExportTo();
            }

            if (GUILayout.Button(
                new GUIContent(
                "Validate",
                "Ensures that the scenario is valid for import into the game engine"),
                GuiHelper.ButtonLayout))
            {
                Validator.Validate(editorScenario, false);
                EditorUtility.DisplayDialog("Validation", "Validation completed. See the console for any warnings/errors", "OK");
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            if (GUILayout.Button(new GUIContent(
                "Create custom..",
                "Creates a custom new scenario"),
                GuiHelper.ButtonLayout))
            {
                CreateWindow.ShowNew();
            }

            if (GUILayout.Button(new GUIContent(
                "Create new",
                "Creates a new scenario with a single sector and player"),
                GuiHelper.ButtonLayout))
            {
                CreateNewScenarioTool.CreateNewSingleSector();
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button(new GUIContent(
                "Create new empty",
                "Creates a new empty scenario"),
                GuiHelper.ButtonLayout))
            {
                CreateNewScenarioTool.CreateNewEmpty();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button(new GUIContent(
                "Import...",
                "Imports a scenario"),
                GuiHelper.ButtonLayout))
            {
                ImportWindow.ShowNew();
            }

            if (GUILayout.Button(
                new GUIContent(
                "Settings",
                "Opens up the settings window"),
                GuiHelper.ButtonLayout))
            {
                SettingsService.OpenProjectSettings(MyCustomSettingsIMGUIRegister.SettingsProviderPath);
            }
        }
    }
}
