using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Utilities
{
    public class ImportExportTool : MonoBehaviour
    {
        [MenuItem("Window/IP Editor V2/Export/Quick Export")]
        public static void FixUpValidateAndExportMenuItem()
        {
            var editorSavedGame = Util.FindSavedGameOrErrorOut();

            FixUpUnitOwnership.SetFleetChildrenToSameFaction(editorSavedGame);
            FixUpUnitOwnership.SetUnitFactionsToPilotFactions(editorSavedGame);

            // Blitz all ids to ensure uniqueness
            AutoAssignIdsTool.ClearAllIds(editorSavedGame);
            AutoAssignIdsTool.AutoAssignIds(editorSavedGame);

            var path = GetExportPath(editorSavedGame.Title);
            ValidateAndExport(editorSavedGame, path);
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
            }
        }
    }
}
