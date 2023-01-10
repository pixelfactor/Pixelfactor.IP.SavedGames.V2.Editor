using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
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

            var editorSavedGame = SavedGameUtil.FindSavedGame();
            var hasScenario = editorSavedGame != null;
            var canPlay = hasScenario && !string.IsNullOrWhiteSpace(CustomSettings.GetOrCreateSettings().GameExecutablePath);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Current scenario", editorSavedGame != null ? editorSavedGame.Title : "[None]");
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

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
                Validator.Validate(editorSavedGame, false);
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            if (GUILayout.Button(new GUIContent(
                "Create new...",
                "Creates a new scenario"),
                GuiHelper.ButtonLayout))
            {
                CreateNewScenarioTool.CreateNew();
                GUIUtility.ExitGUI();
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
