using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Main.Export;
using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Export
{
    public static class SavedGameExporter
    {
        public static SavedGame Export(EditorScenario editorScenario)
        {
            Debug.Log($"Exporting scenario [{(!string.IsNullOrEmpty(editorScenario.Title) ? editorScenario.Title : "Unnamed")}]");

            var exportOperation = new ExportOperation();
            var settings = CustomSettings.GetOrCreateSettings();
            var savedGame = exportOperation.Export(editorScenario, settings);

            Debug.Log($"Adding unit ammo...");
            new AddAmmoTool(settings).AddAmmo(editorScenario, savedGame, settings.Export_AutoAddAmmo);

            Debug.Log($"Export completed successfully");

            return savedGame;

        }
    }
}
