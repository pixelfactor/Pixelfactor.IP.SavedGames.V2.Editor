using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
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
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();
            if (editorSavedGame == null)
                return;

            ImportExportTool.QuickFixSavedGame(editorSavedGame);

            // Validate first
            try
            {
                Validator.Validate(editorSavedGame, true);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(new System.Exception("Validation failed. Export will not continue. Please contact Pixelfactor for support.", ex));
                EditorUtility.DisplayDialog("Failed validation", "Validation failed. See the console for more details", "OK");
                return;
            }

            if (TryGetExePath(out string exePath))
            {
                try
                {
                    Debug.Log("Attempting to export and run scenario");

                    var savedGame = SavedGameExporter.Export(editorSavedGame);
                    var exportPath = ImportExportTool.GetExportPath(editorSavedGame.Title);

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
