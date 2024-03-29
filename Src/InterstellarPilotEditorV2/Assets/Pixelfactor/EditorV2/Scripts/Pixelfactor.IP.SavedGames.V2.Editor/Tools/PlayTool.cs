﻿using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Export;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public class PlayTool : MonoBehaviour
    {
        public static void Play()
        {
            var editorScenario = SavedGameUtil.FindSavedGameOrErrorOut();
            if (editorScenario == null)
                return;

            ImportExportTool.QuickFixSavedGame(editorScenario);

            // Validate first
            try
            {
                Validator.Validate(editorScenario, true);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(new System.Exception("Validation failed. Export will not continue. Please contact Pixelfactor for support.", ex));
                EditorUtility.DisplayDialog("Failed validation", "Validation failed. See the console for more details", "OK");
                return;
            }

            if (TryGetExePath(out string exePath))
            {
                if (!System.IO.File.Exists(exePath))
                {
                    EditorUtility.DisplayDialog("Run scenario", "The path to the game's executable does not exist. Open settings to configure the executable path", "OK");
                    return;
                }
                try
                {
                    Debug.Log("Attempting to export and run scenario");

                    var savedGame = SavedGameExporter.Export(editorScenario);
                    var exportPath = ImportExportTool.GetExportPath(editorScenario.Title);

                    BinarySerialization.Writers.SaveGameWriter.WriteToPath(savedGame, exportPath);

                    Debug.Log($"Save file successfully wrote to {exportPath}");

                    var arguments = $"-scenarioPath \"{exportPath}\"";
                    var processStartInfo = new System.Diagnostics.ProcessStartInfo(exePath, arguments);
                    System.Diagnostics.Process.Start(processStartInfo);
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(new System.Exception("Failed to run game", ex));
                    EditorUtility.DisplayDialog("Run scenario", "An error occured while attempting to run the game. Inspect the console for more details.", "OK");
                }
            }
        }

        static bool TryGetExePath(out string exePath)
        {
            exePath = null;

            var settings = CustomSettings.GetOrCreateSettings();
            if (string.IsNullOrWhiteSpace(settings.GameExecutablePath))
            {
                EditorUtility.DisplayDialog("Run scenario", "The IP2 executable has not been set. Enter the path of the executable in Project Settings -> IP2 Editor -> IP2 Exe path", "OK");
                return false;
            }

            exePath = settings.GameExecutablePath;
            return true;
        }
    }
}
