using Pixelfactor.IP.SavedGames.V2.Model;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Main.Import
{
    public static class ImportTool
    {
        public static void ImportFromFile(string filePath, EditorScenario editorScenario)
        {
            var saveModel = BinaryReadTool.ReadFromFile(filePath, out _);
            ImportFromModel((SavedGame)saveModel, editorScenario);
        }

        public static void ImportFromModel(SavedGame savedGameModel, EditorScenario editorScenario)
        {
            new ImportOperation(savedGameModel, editorScenario).Import();
        }
    }
}
