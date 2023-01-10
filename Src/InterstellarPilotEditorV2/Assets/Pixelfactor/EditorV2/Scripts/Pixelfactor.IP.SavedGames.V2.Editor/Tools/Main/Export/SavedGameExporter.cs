using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Export
{
    public static class SavedGameExporter
    {
        public static SavedGame Export(EditorScenario editorScenario)
        {
            Debug.Log($"Exporting scenario [{(!string.IsNullOrEmpty(editorScenario.Title) ? editorScenario.Title : "Unnamed")}]");

            var options = editorScenario.GetComponentInChildren<SavedGameExportOptions>();
            if (options == null)
            {
                options = editorScenario.gameObject.AddComponent<SavedGameExportOptions>();
            }

            var exportOperation = new ExportOperation();
            var savedGame = exportOperation.Export(editorScenario, options);


            Debug.Log($"Export completed successfully");

            return savedGame;

        }
    }
}
