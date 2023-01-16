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
            var editorScenario = SavedGameUtil.FindSavedGameOrErrorOut();

            QuickFixSavedGame(editorScenario);

            var path = GetExportPath(editorScenario.Title);
            ValidateAndExport(editorScenario, path);
        }

        public static void FixUpAndExportTo()
        {
            var editorScenario = SavedGameUtil.FindSavedGameOrErrorOut();

            QuickFixSavedGame(editorScenario);

            var path = EditorUtility.SaveFilePanel(
                     "Save scenario to..",
                     "",
                     $"{editorScenario.Title}.dat",
                     "dat");

            if (string.IsNullOrWhiteSpace(path))
                return;

            ValidateAndExport(editorScenario, path);
        }

        public static void QuickFixSavedGame(EditorScenario editorScenario)
        {
            var settings = CustomSettings.GetOrCreateSettings();

            FixUpUnitOwnership.SetFleetChildrenToSameFaction(editorScenario);
            FixUpUnitOwnership.SetUnitFactionsToPilotFactions(editorScenario);

            if (settings.Export_RemoveUntargettedWormholes)
            { 
                RemoveUntargettedWormholesTool.Remove(editorScenario);
            }

            SeedTool.AutosetSeedsIfPreferred(editorScenario);

            // Blitz all ids to ensure uniqueness
            AutoAssignIdsTool.ClearAllIds(editorScenario);
            AutoAssignIdsTool.AutoAssignIds(editorScenario);
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

        private static void ValidateAndExport(EditorScenario editorScenario, string path)
        {
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

            Model.SavedGame savedGame = null;
            try
            {
                savedGame = SavedGameExporter.Export(editorScenario);
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
