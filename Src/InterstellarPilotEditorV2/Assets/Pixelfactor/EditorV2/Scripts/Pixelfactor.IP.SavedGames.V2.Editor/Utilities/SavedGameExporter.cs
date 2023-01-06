using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Utilities
{
    public static class SavedGameExporter
    {
        public const float SECTOR_SIZE = 16000f;

        public static SavedGame Export(EditorSavedGame editorSavedGame)
        {
            Debug.Log($"Exporting scenario [{(!string.IsNullOrEmpty(editorSavedGame.Title) ? editorSavedGame.Title : "Unnamed")}]");

            var options = editorSavedGame.GetComponentInChildren<SavedGameExportOptions>();
            if (options == null)
            {
                options = editorSavedGame.gameObject.AddComponent<SavedGameExportOptions>();
            }

            var exportOperation = new ExportOperation();
            var savedGame = exportOperation.Export(editorSavedGame, options);


            Debug.Log($"Export completed successfully");

            return savedGame;

        }
    }
}
