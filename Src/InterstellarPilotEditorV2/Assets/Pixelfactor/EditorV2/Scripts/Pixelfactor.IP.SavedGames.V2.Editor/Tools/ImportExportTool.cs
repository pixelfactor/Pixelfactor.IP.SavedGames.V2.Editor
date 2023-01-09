using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Export;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Wormholes;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public class ImportExportTool : MonoBehaviour
    {
        public static void FixUpValidateAndExport()
        {
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();

            QuickFixSavedGame(editorSavedGame);

            var path = GetExportPath(editorSavedGame.Title);
            ValidateAndExport(editorSavedGame, path);
        }

        public static void FixUpAndExportTo()
        {
            var editorSavedGame = SavedGameUtil.FindSavedGameOrErrorOut();

            QuickFixSavedGame(editorSavedGame);

            var path = EditorUtility.SaveFilePanel(
                     "Save scenario to..",
                     "",
                     $"{editorSavedGame.Title}.dat",
                     "dat");

            ValidateAndExport(editorSavedGame, path);
        }

        public static void QuickFixSavedGame(EditorSavedGame editorSavedGame)
        {
            var settings = CustomSettings.GetOrCreateSettings();

            FixUpUnitOwnership.SetFleetChildrenToSameFaction(editorSavedGame);
            FixUpUnitOwnership.SetUnitFactionsToPilotFactions(editorSavedGame);

            if (settings.Export_RemoveUntargettedWormholes)
            { 
                RemoveUntargettedWormholesTool.Remove(editorSavedGame);
            }

            SeedTool.AutosetSeedsIfPreferred(editorSavedGame);

            // Blitz all ids to ensure uniqueness
            AutoAssignIdsTool.ClearAllIds(editorSavedGame);
            AutoAssignIdsTool.AutoAssignIds(editorSavedGame);
        }

        public static string GetExportPath(string scenarioTitle = "")
        {
            var preferredPath = CustomSettings.GetOrCreateSettings().DefaultExportPath;

            if (string.IsNullOrWhiteSpace(preferredPath))
            {
                preferredPath = System.IO.Path.GetDirectoryName(FileUtil.GetUniqueTempPathInProject());
            }

            var name = !string.IsNullOrEmpty(scenarioTitle) ? scenarioTitle : "NewEditorSavedGame";

            var fileName = $"{name}.dat";
            var path = Path.Combine(preferredPath, fileName);

            return path;
        }

        private static void ValidateAndExport(EditorSavedGame editorSavedGame, string path)
        {
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

            Model.SavedGame savedGame = null;
            try
            {
                savedGame = SavedGameExporter.Export(editorSavedGame);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(new System.Exception("SaveFile Export failed. Please contact Pixelfactor for support.", ex));
                EditorUtility.DisplayDialog("Failed export", "Export failed. See the console for more details", "OK");
                return;
            }

            try
            {
                BinarySerialization.Writers.SaveGameWriter.WriteToPath(savedGame, path);

                Debug.Log($"Save file successfully wrote to {path}");

                EditorUtility.RevealInFinder(path);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(new System.Exception("SaveFile Write failed. Please contact Pixelfactor for support.", ex));
                EditorUtility.DisplayDialog("Failed file write", "Failed to write file. See the console for more details", "OK");
            }
        }
    }
}
